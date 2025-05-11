namespace ImageTemplate
{
    // Custom UnionFind DS implemenation -- based on Alogrithms 4th edtion implementation
    public class UnionFind
    {

        public int[] Parent { get => _parent; }
        private int[] _parent;

        // removing rank array would result in `Find` operation being O(nlog(n)) instead of amortized ~O(1)
        private int[] _rank;

        public UnionFind(int size)
        {
            _parent = new int[size];
            _rank = new int[size];

            for (int i = 0; i < size; i++)
                _parent[i] = i;
        }

        public int[] FlattenParent()
        {
            for (int i = 0; i < _parent.Length; i++)
                _parent[i] = Find(i);

            return _parent;
        }

        public int Find(int p)
        {
            while (p != _parent[p])
            {
                // Without path compression `Find` would be a O(nlog(n)) operation

                // Path compression
                _parent[p] = _parent[_parent[p]];
                p = _parent[p];
            }

            return p;
        }

        public void Union(int p, int q)
        {
            int pID = Find(p);
            int qID = Find(q);

            if (pID == qID) return;

            if (_rank[pID] < _rank[qID])
            {
                _parent[pID] = _parent[qID];
            }
            else if (_rank[pID] > _rank[qID])
            {
                _parent[qID] = _parent[pID];
            }
            else
            {
                _rank[pID]++;
                _parent[qID] = _parent[pID];
            }
        }
    }
}
