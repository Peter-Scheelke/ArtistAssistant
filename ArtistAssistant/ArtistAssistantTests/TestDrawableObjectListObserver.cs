using ArtistAssistant.DrawableObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistAssistantTests
{
    class TestDrawableObjectListObserver : IObserver<DrawableObjectList>
    {
        public IDisposable Unsubscriber { get; set; }

        public TestDrawableObjectListObserver()
        {
            this.WasNotified = false;
        }

        public bool WasNotified
        {
            get; set;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(DrawableObjectList value)
        {
            this.WasNotified = true;
        }
    }
}
