using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookSleeve;

namespace Prime.Optimus
{
    //todo нужен ли?
    class OptimusRecieverService : IOptimusService
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IOptimusIn[] OptimusIn
        {
            get { throw new NotImplementedException(); }
        }

        public IOptimusOut[] OptimusOut
        {
            get { throw new NotImplementedException(); }
        }

        public RedisConnection Connection
        {
            get { throw new NotImplementedException(); }
        }

        public int DbPage
        {
            get { throw new NotImplementedException(); }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
