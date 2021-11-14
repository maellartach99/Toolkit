using System;
using System.Collections;
using System.Collections.Generic;

namespace Pokemon.Data.Exceptions
{
    public class MissingAttributeException<T> : Exception
    {
        private string instance;

        public MissingAttributeException(string instance)
        {
            this.instance = instance;
        }
    }

    public class InvalidAttributeException<T> : Exception
    {
        private string instance;

        public InvalidAttributeException(string instance)
        {
            this.instance = instance;
        }
    }
}

