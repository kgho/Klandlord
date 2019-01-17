﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AhpilyServer
{

    public delegate void ExecuteDelegate();
    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExecute
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static SingleExecute instance = null;
        public static SingleExecute Instance
        {
            get
            {
                lock(o)
                {
                    if (instance == null)
                        instance = new SingleExecute();
                    return instance;
                }
            }
        }

        private static object o=1;

        /// <summary>
        /// 互斥锁
        /// </summary>
        public Mutex mutex;

        public SingleExecute()
        {
            mutex = new Mutex();
        }

        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="executeDelegate"></param>
        public void Execute(ExecuteDelegate executeDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();
                executeDelegate();
                mutex.ReleaseMutex();
            }
        }
    }
}
