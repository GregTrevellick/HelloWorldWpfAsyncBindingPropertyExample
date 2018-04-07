using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace AsyncBindingProperty
{
    public class SlowSource : INotifyPropertyChanged
    {
        private volatile string dataValue = "Initial data";
        private int id = 1;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Data
        {
            get
            {
                Debug.WriteLine("Get thread: " + Thread.CurrentThread.ManagedThreadId);
                return dataValue;
            }
            set
            {
                Debug.WriteLine("Set thread: " + Thread.CurrentThread.ManagedThreadId);
                if (value != dataValue)
                {
                    dataValue = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        public void FetchNewData()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Debug.WriteLine("Worker thread: " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(TimeSpan.FromSeconds(5));

                string newValue = "Value " + Interlocked.Increment(ref id);
                this.Data = newValue;
            });
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                Debug.WriteLine("Event thread: " + Thread.CurrentThread.ManagedThreadId);
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
