using System;

namespace ImageTemplate
{
    /* Custom UnionFind DS implemenation -- based on Alogrithms 4th edtion implementation
       with internal difference (Int) calculations.

       Internal Difference:
        The internal difference of a component C⊆V is defined as the largest weight edge that is 
        necessarily to keep the component connected. That is, without this edge, the component becomes 
        disconnected. In other word, larger edges are not necessary to keep the component connected 
        while smaller edges are necessary to keep it connected.

        Int⁡(C)= Max_(ⅇ∈C)(w(e))
    */
    public class UnionFind
    {
        // Storing 4 dense pixel arrays, is very memory intensive.
        //  - `size`: could be removed, but calculating size would result in an O(n) operation
        //  - `rank`: removing rank array would result in `Find` operation being O(nlog(n)) instead of ~O(1)

        private int[] _parent;
        private int[] _size;
        private int[] _rank;

        // We only care abount the smallest connectivity edge weight and not the whole MST.
        private float[] _internalDiff;

        public UnionFind(int size)
        {
            _parent = new int[size];
            _rank = new int[size];
            _size = new int[size];
            _internalDiff = new float[size];

            for (int i = 0; i < size; i++)
            {
                _parent[i] = i;
                _size[i] = 1;
            }
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

        public int Size(int p)
        {
            return _size[Find(p)];
        }

        public float InternalDiff(int p)
        {
            return _internalDiff[Find(p)];
        }

        public void Union(int p, int q, int weight)
        {
            int pID = Find(p);
            int qID = Find(q);

            if (pID == qID) return;

            float newInternalDiff = Math.Max(weight, Math.Max(_internalDiff[pID], _internalDiff[qID]));
            if (_rank[pID] < _rank[qID])
            {
                _internalDiff[qID] = newInternalDiff;
                _size[qID] += _size[pID];

                _parent[pID] = _parent[qID];
            }
            else if (_rank[pID] > _rank[qID])
            {
                _internalDiff[pID] = newInternalDiff;
                _size[pID] += _size[qID];

                _parent[qID] = _parent[pID];
            }
            else
            {
                _internalDiff[pID] = newInternalDiff;
                _size[pID] += _size[qID];
                _rank[pID]++;

                _parent[qID] = _parent[pID];
            }
        }
    }
}
