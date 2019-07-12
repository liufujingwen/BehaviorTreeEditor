// EYUGame Socket Client library
// Copyright (C) Ramon@eyugame.com

using System;
using System.Collections.Generic;
using System.IO;

namespace Serializable.IO
{
    /// <summary>
    /// Byte buffer.
    /// </summary>
    public class SerializableByteBuffer
    {
        /// <summary>
        /// Wrap the specified value.
        /// </summary>
        /// <param name="value">Value.</param>
        public static SerializableByteBuffer Wrap(byte[] value)
        {
            SerializableByteBuffer result = new SerializableByteBuffer(value);
            return result;
        }

        #region

        /// <summary>
        /// Initializes a new instance of the <see cref="System.IO.SerializableByteBuffer"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        private SerializableByteBuffer(byte[] value)
        {
            this.capacity = value.Length;
            this.limit = value.Length;
            this.position = 0;
            this.chunks = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.IO.SerializableByteBuffer"/> class.
        /// </summary>
        /// <param name="capacity">Capacity.</param>
        public SerializableByteBuffer(int capacity)
        {
            this.chunks = new byte[capacity];
            this.capacity = capacity;
        }

        #endregion

        #region

        private int position = 0;
        private int limit = 0;
        private int capacity = 0;
        private byte[] chunks;
        private int mark = -1;

        #endregion


        #region Get / Set

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public int Position
        {
            get { return position; }
            set { this.position = value; }
        }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>The limit.</value>
        public int Limit
        {
            get { return limit; }
            set { this.limit = value; }
        }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get { return capacity; }
            set { ResizeChunk(capacity); }
        }

        /// <summary>
        /// Gets the chunks.
        /// </summary>
        /// <value>The chunks.</value>
        public byte[] Chunks
        {
            get { return chunks; }
        }

        /// <summary>
        /// Gets the remaining.
        /// </summary>
        /// <value>The remaining.</value>
        public int Remaining
        {
            get { return limit - position; }
        }

        #endregion


        #region Offsets


        /// <summary>
        /// Mark position.
        /// </summary>
        public int Mark()
        {
            mark = position;
            return mark;
        }

        /// <summary>
        /// Reset position.
        /// </summary>
        public int Reset()
        {
            this.position = this.mark;
            this.mark = -1;
            return this.position;
        }


        /**
		* rewind makes a buffer ready for re-reading the data that it already contains:
		* It leaves the limit unchanged and sets the position to zero.
		*/
        public SerializableByteBuffer Rewind()
        {
            position = 0;
            return this;
        }

        /**
		* flip makes a buffer ready for a new sequence of channel-write or relative get operations:
		* It sets the limit to the current position and then sets the position to zero.
		*/
        public SerializableByteBuffer Flip()
        {
            limit = position;
            position = 0;
            return this;
        }

        /**
		* clear makes a buffer ready for a new sequence of channel-read or relative put operations:
		* It sets the limit to the capacity and the position to zero.
		*/
        public SerializableByteBuffer Clear()
        {
            limit = capacity;
            position = 0;
            return this;
        }

        #endregion

        /// <summary>
        /// Resizes the chunk.
        /// </summary>
        /// <param name="capacity">Capacity.</param>
        private void ResizeChunk(int capacity)
        {
            if (this.capacity < capacity)
            {
                byte[] values = new byte[capacity];
                Buffer.BlockCopy(chunks, 0, values, 0, this.capacity);
                this.chunks = values;
            }
            this.capacity = capacity;
            this.limit = 0;
        }

        /// <summary>
        /// Writes the check.
        /// </summary>
        /// <param name="size">Size.</param>
        private void WriteCheck(int size)
        {
            if (position + size > capacity)
            {
                ResizeChunk(capacity + capacity);
            }
        }

        /// <summary>
        /// Reads the check.
        /// </summary>
        /// <param name="size">Size.</param>
        private void ReadCheck(int size)
        {
            if (position + size > limit)
            {
                throw new IndexOutOfRangeException("Position:" + (position + size) + ", Limit:" + limit + " Out of range!!");
            }
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="length">Length.</param>
        public void readBytes(byte[] target, int offset, int length)
        {
            ReadCheck(length);

            Buffer.BlockCopy(chunks, position, target, offset, length);
            position += length;
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="length">Length.</param>
        public void writeBytes(byte[] value, int offset, int length)
        {
            WriteCheck(length);

            Buffer.BlockCopy(value, offset, chunks, position, length);
            position += length;
        }

        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <returns>The byte.</returns>
        public byte readByte()
        {
            ReadCheck(1);

            byte value = chunks[position++];
            return value;
        }

        /// <summary>
        /// Writes the byte.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeByte(byte value)
        {
            WriteCheck(1);

            chunks[position++] = value;
        }

        /// <summary>
        /// Reads the short.
        /// </summary>
        /// <returns>The short.</returns>
        public short readShort()
        {
            ReadCheck(2);

            short value = (short)((chunks[position++] & 0xFF) << 8
                          | (chunks[position++] & 0xFF));
            return value;
        }

        /// <summary>
        /// Writes the short.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeShort(short value)
        {
            WriteCheck(2);

            chunks[position++] = (byte)(value >> 8);
            chunks[position++] = (byte)(0xFF & value);
        }

        /// <summary>
        /// Reads the int.
        /// </summary>
        /// <returns>The int.</returns>
        public int readInt()
        {
            ReadCheck(4);

            return chunks[position++] << 24
            | (chunks[position++] & 0xFF) << 16
            | (chunks[position++] & 0xFF) << 8
            | (chunks[position++] & 0xFF);
        }

        /// <summary>
        /// Writes the int.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeInt(int value)
        {
            WriteCheck(4);

            chunks[position++] = (byte)(0xFF & (value >> 24));
            chunks[position++] = (byte)(0xFF & (value >> 16));
            chunks[position++] = (byte)(0xFF & (value >> 8));
            chunks[position++] = (byte)(0xFF & value);
        }

        /// <summary>
        /// Writes the int.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeInt(uint value)
        {
            WriteCheck(4);

            chunks[position++] = (byte)(0xFF & (value >> 24));
            chunks[position++] = (byte)(0xFF & (value >> 16));
            chunks[position++] = (byte)(0xFF & (value >> 8));
            chunks[position++] = (byte)(0xFF & value);
        }

        /// <summary>
        /// Reads the long.
        /// </summary>
        /// <returns>The long.</returns>
        public long readLong()
        {
            ReadCheck(8);

            long s0 = (chunks[position++] & 0xFF) << 56;
            long s1 = (chunks[position++] & 0xFF) << 48;
            long s2 = (chunks[position++] & 0xFF) << 40;
            long s3 = (chunks[position++] & 0xFF) << 32;
            long s4 = (chunks[position++] & 0xFF) << 24;
            long s5 = (chunks[position++] & 0xFF) << 16;
            long s6 = (chunks[position++] & 0xFF) << 8;
            long s7 = chunks[position++] & 0xFF;

            return s0 | s1 | s2 | s3 | s4 | s5 | s6 | s7;

        }

        /// <summary>
        /// Writes the long.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeLong(long value)
        {
            WriteCheck(8);

            chunks[position++] = (byte)(0xFF & (value >> 56));
            chunks[position++] = (byte)(0xFF & (value >> 48));
            chunks[position++] = (byte)(0xFF & (value >> 40));
            chunks[position++] = (byte)(0xFF & (value >> 32));
            chunks[position++] = (byte)(0xFF & (value >> 24));
            chunks[position++] = (byte)(0xFF & (value >> 16));
            chunks[position++] = (byte)(0xFF & (value >> 8));
            chunks[position++] = (byte)(0xFF & value);
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <returns>The float.</returns>
        public float readFloat()
        {
            ReadCheck(4);

            for (int i = 0; i < 2; i++)
            {
                int fromIndex = position + i;
                int toIndex = position + 3 - i;
                byte b = chunks[fromIndex];
                chunks[fromIndex] = chunks[toIndex];
                chunks[toIndex] = b;
            }

            float value = BitConverter.ToSingle(chunks, position);
            position += 4;
            return value;
        }

        /// <summary>
        /// Writes the float.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeFloat(float value)
        {
            WriteCheck(4);

            byte[] bits = BitConverter.GetBytes(value);
            for (int i = 3; i >= 0; i--)
                chunks[position++] = bits[i];
            //foreach (byte b in bits)
            //{
            //    chunks[position++] = b;
            //}
        }

        /// <summary>
        /// Reads the double.
        /// </summary>
        /// <returns>The double.</returns>
        public double readDouble()
        {
            ReadCheck(8);

            for (int i = 0; i < 4; i++)
            {
                int fromIndex = position + i;
                int toIndex = position + 7 - i;
                byte b = chunks[fromIndex];
                chunks[fromIndex] = chunks[toIndex];
                chunks[toIndex] = b;
            }

            double value = BitConverter.ToDouble(chunks, position);
            position += 8;
            return value;
        }

        /// <summary>
        /// Writes the double.
        /// </summary>
        /// <param name="value">Value.</param>
        public void writeDouble(double value)
        {
            WriteCheck(8);

            byte[] bits = BitConverter.GetBytes(value);
            for (int i = 7; i >= 0; i--)
                chunks[position++] = bits[i];

            //foreach (byte b in bits)
            //{
            //    chunks[position++] = b;
            //}
        }

    }
}

