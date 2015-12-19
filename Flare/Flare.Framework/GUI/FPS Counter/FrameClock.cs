using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flare.Framework.GUI
{
    internal class FrameClock
    {
        private SampleData Sample;
        private Stopwatch timer;
        private Range<long, long> Time;
        private Range<float, uint> Freq;

        // The number of frames to be sampled for averaging.
        internal int Depth
        {
            get
            {
                return Sample.Depth;
            }
            set
            {
                Sample.Depth = value;
            }
        }

        // The total accumulated frame time.
        internal long TotalFrameTime
        {
            get
            {
                return Time.elapsed;
            }
        }

        // The total accumulated number of frames.
        internal ulong TotalFrameCount
        {
            get
            {
                return Freq.elapsed;
            }
        }

        // Time elapsed during the last 'FrameClock::beginFrame/endFrame' pair.
        internal long LastFrameTime
        {
            get
            {
                return Time.current;
            }
        }

        // The shortest measured frame time.
        internal long MinFrameTime
        {
            get { return Time.minimum; }
        }

        // The longest measured frame time.
        internal long MaxFrameTime
        {
            get
            {
                return Time.maximum;
            }
        }

        // Average frame time over the last Sample.Depth frames.
        internal long AverageFrameTime
        {
            get
            {
                return Time.average;
            }
        }

        // Returns: Frames per second, considering the pervious frame only.
        internal float FPS
        {
            get
            {
                return Freq.current;
            }
        }

        // Returns: The lowest measured frames per second.
        internal float MinFPS
        {
            get
            {
                return Freq.minimum;
            }
        }

        // Returns: The highest measured frames per second.
        internal float MaxFPS
        {
            get
            {
                return Freq.maximum;
            }
        }

        // Returns: Average frames per second over the last getSampleDepth() frames.
        internal float AverageFPS
        {
            get
            {
                return Freq.average;
            }
        }


        internal FrameClock(int depth = 100)
        {
            Sample = new SampleData();
            Depth = depth;
            timer = Stopwatch.StartNew();
        }

        /* Begin frame timing.
         * Should be called once at the start of each new frame. 
         */
        internal void BeginFrame()
        {
            timer.Restart();
        }


        /* End frame timing.
         * Should be called once at the end of each frame.
         * Returns: Time elapsed since the matching FrameClock.BeginFrame.
         */
        internal float EndFrame()
        {
            timer.Stop();
            Time.current = timer.ElapsedMilliseconds;

            Sample.Accumulator -= Sample.Data[Sample.Index];
            Sample.Data[Sample.Index] = Time.current;
            Sample.TotalSamples = Sample.TotalSamples + 1;

            Sample.Accumulator += Time.current;
            Time.elapsed += Time.current;

            if (++Sample.Index >= Sample.Depth)
            {
                Sample.Index = 0;
            }

            if (Time.current > Math.E) // Time is about 0, accounting for float imprecision
            {
                Freq.current = 1.0f / (Time.current / 1000f); // Convert ms to hertz
            }

            if (Sample.Accumulator > Math.E)
            {
                float freqSmooth = (float)Sample.Size;
                Freq.average = freqSmooth / (Sample.Accumulator / 1000f);
            }


            long smooth = (long)Sample.Size;
            Time.average = Sample.Accumulator / smooth;

            if (Freq.current < Freq.minimum)
                Freq.minimum = Freq.current;
            if (Freq.current > Freq.maximum)
                Freq.maximum = Freq.current;

            if (Time.current < Time.minimum)
                Time.minimum = Time.current;
            if (Time.current > Time.maximum)
                Time.maximum = Time.current;

            ++Freq.elapsed;

            return Time.current;
        }

        /* Resets all times to zero and discards accumulated samples.
         */
        internal void Clear()
        {
            Sample = new SampleData();
        }

        internal class SampleData
        {
            internal long[] Data;
            internal int Index = 0, Depth = 0;
            internal long Accumulator = 0;

            private int totalSamples;

            // Represents size of data set, calculated from the smaller of the depth or total samples
            internal long Size
            {
                get
                {
                    return TotalSamples < Depth ? TotalSamples : Depth;
                }
            }

            internal int TotalSamples
            {
                get { return totalSamples; }
                set
                {
                    totalSamples++;
                    if (totalSamples == int.MaxValue)
                        totalSamples = Depth;
                }
            }

            internal SampleData(int depth = 100)
            {
                Depth = depth;
                Data = new long[depth];
            }

            internal void Resize(int depth)
            {
                TotalSamples = depth;
                Depth = depth;
                Index = 0;
                long[] temp = new long[depth];
                // Copy most recent data into new array
                for (int i = 0, j = Index + 1; i < depth; i++, j++)
                {
                    if (j >= Data.Length)
                        j = 0;
                    temp[i] = Data[j];
                }
                Data = temp;
                Accumulator = 0;
                for (int i = 0; i < Data.Length; i++)
                {
                    Accumulator += Data[i];
                }
            }
        }

        struct Range<T, U>
        {
            internal Range(T min, T max, T avg, T cur, U elapsed)
            {
                this.minimum = min;
                this.maximum = max;
                this.average = avg;
                this.current = cur;
                this.elapsed = elapsed;
            }
            internal void swap(Range<T, U> other)
            {
                T temp = this.minimum;
                this.minimum = other.minimum;
                other.minimum = temp;

                temp = this.maximum;
                this.maximum = other.maximum;
                other.maximum = temp;

                temp = this.current;
                this.current = other.current;
                other.current = temp;

                temp = this.average;
                this.average = other.average;
                other.average = temp;

                U temp2 = this.elapsed;
                this.elapsed = other.elapsed;
                other.elapsed = temp2;
            }
            internal T minimum;
            internal T maximum;
            internal T average;
            internal T current;
            internal U elapsed;
        };
    }
}