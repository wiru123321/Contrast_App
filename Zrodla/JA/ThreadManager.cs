using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace JA
{
    class ThreadManager
    {

        private int noOfThreads;

        bool isCppChecked;

        List<Thread> threads;
        private EdgeDetection edgeDetection;

        public ThreadManager(int value, bool cpp, ref EdgeDetection form1)
        {
            this.noOfThreads = value;
            this.isCppChecked = cpp;
            this.edgeDetection = form1;
        }

        private void CreateThread(int begin, int end)
        {
            if (isCppChecked)
            {
                var t = new Thread(() => edgeDetection.RunCppDll(begin, end));
                threads.Add(t);
            }

            else
            {
                var t = new Thread(() => edgeDetection.RunAsmDll(begin, end));
                threads.Add(t);
            }
        }


        public void CreateThreadsSet()
        {
            const int VECTOR_LENGTH = 16;
            int vectorsPerThread = edgeDetection.getNoOfVectors() / noOfThreads;
            threads = new List<Thread>();

            int threadStep = vectorsPerThread * VECTOR_LENGTH;
            int begin = 0;
            int end = 0;

            for (int i = 0; i < noOfThreads; ++i)
            {
                begin = end;
                end += threadStep;

                CreateThread(begin, end);
            }

        }

        
        public String RunThreads()
        {
            Stopwatch clock = new Stopwatch();

            clock.Start();
            foreach (Thread th in threads)
            {
                th.Start();
            }

            foreach (Thread th in threads)
            {
                th.Join();
            }
            clock.Stop();


            return clock.ElapsedMilliseconds.ToString();
        }
    }
}

