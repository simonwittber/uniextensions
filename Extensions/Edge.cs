namespace DifferentMethods.Extensions
{
    public struct Edge
    {
        public int A, B;
        public Edge(int a, int b)
        {
            if (a < b)
            {
                A = a;
                B = b;
            }
            else
            {
                A = b;
                B = a;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Edge))
                return false;
            var e = (Edge)obj;
            return (e.A == A && e.B == B);
        }

        public override int GetHashCode()
        {
            return A ^ 524287 ^ B;
        }
    }

}