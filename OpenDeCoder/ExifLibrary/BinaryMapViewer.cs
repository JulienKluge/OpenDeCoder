using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace ExifLibrary
{
#if DEBUG
    /// <summary>
    /// Represents a stream of bins.
    /// </summary>
    public interface IBinStream
    {
        /// <summary>
        /// Reads the next bin in the stream.
        /// </summary>
        Bin Read();
        /// <summary>
        /// Writes a bin to the current position.
        /// </summary>
        void Write(Bin bin);
        /// <summary>
        /// Seeks to the given offset from the given position.
        /// </summary>
        void Seek(long offset, SeekOrigin origin);
        /// <summary>
        /// Seeks to the start of the next bin from the current position.
        /// </summary>
        void SeekBin();
        /// <summary>
        /// Gets or sets the position of the stream.
        /// </summary>
        long Position { get; set; }
        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        long Length { get; }
        /// <summary>
        /// Indicates that the end of stream is reached.
        /// </summary>
        bool EOF { get; }
    }

    /// <summary>
    /// Represents a memory stream of bins.
    /// </summary>
    public class MemoryBinStream : IBinStream
    {
        private SortedList<long, Bin> list;
        protected long mPosition;

        public MemoryBinStream()
        {
            list = new SortedList<long, Bin>();
            mPosition = 0;
        }

        /// <summary>
        /// Reads the next bin in the stream.
        /// </summary>
        public Bin Read()
        {
            // Find and return the bin
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (mPosition >= obj.Key && mPosition < obj.Key + obj.Value.Length)
                {
                    long offset = obj.Key;
                    mPosition = offset + obj.Value.Length;
                    return obj.Value;
                }
            }

            // Return a null bin
            long start = 0;
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (obj.Key + obj.Value.Length <= mPosition)
                {
                    start = obj.Key + obj.Value.Length;
                }
            }
            long end = 0;
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (obj.Key > mPosition)
                {
                    end = obj.Key;
                    break;
                }
            }
            mPosition = start;
            Bin bin = new Bin("Null", 0, end - start);
            bin.Offset = mPosition;
            mPosition += bin.Length;
            return bin;
        }

        /// <summary>
        /// Writes a bin to the current position.
        /// </summary>
        public void Write(Bin bin)
        {
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if ((mPosition >= obj.Key) && (mPosition < obj.Key + obj.Value.Length))
                    throw new Exception("Cannot overwrite stream.");
            }
            bin.Offset = mPosition;
            list.Add(mPosition, bin);
            mPosition += bin.Length;
        }

        /// <summary>
        /// Seeks to the given offset from the given position.
        /// </summary>
        public void Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                mPosition = offset;
            else if (origin == SeekOrigin.End)
                mPosition = Length - offset;
            else
                mPosition += offset;
        }

        /// <summary>
        /// Seeks to the start of the next bin from the current position.
        /// </summary>
        public void SeekBin()
        {
            foreach (KeyValuePair<long, Bin> obj in list)
            {
                if (obj.Key > mPosition)
                {
                    mPosition = obj.Key;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the stream.
        /// </summary>
        public long Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        public long Length
        {
            get
            {
                if (list.Count == 0)
                    return 0;

                long length = 0;
                foreach (KeyValuePair<long, Bin> obj in list)
                {
                    length = obj.Key + obj.Value.Length;
                }

                return length;
            }
        }

        /// <summary>
        /// Indicates that the end of stream is reached.
        /// </summary>
        public bool EOF
        {
            get
            {
                return (mPosition >= Length);
            }
        }
    }

    /// <summary>
    /// Represents a bin of given size.
    /// </summary>
    public struct Bin
    {
        /// <summary>
        /// Returns the hash code for this bin.
        /// </summary>
        public override int GetHashCode()
        {
            return Offset.GetHashCode();
        }

        /// <summary>
        /// Gets the offset of this bin from the start of stream.
        /// </summary>
        public long Offset { get; internal set; }
        /// <summary>
        /// Gets or sets the name of the bin.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the marker id used for displaying the bin.
        /// </summary>
        public byte Marker { get; set; }
        /// <summary>
        /// Gets or sets the length of the bin.
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// Gets or sets the user-defined data associated with this bin.
        /// </summary>
        public object Tag { get; set; }

        public Bin(string name, byte marker, long length, object tag)
            : this()
        {
            Name = name;
            Marker = marker;
            Length = length;
            Tag = tag;
        }

        public Bin(string name, byte marker, long length)
            : this(name, marker, length, null)
        {
            ;
        }
    }
#endif
}
