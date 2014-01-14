using System;

namespace Prime
{
    [Serializable]
    public class Token
    {
        private Token()
        {
        }

        public static readonly Token Empty = new Token();
    }
}