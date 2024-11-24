using System.Collections.Generic;
using UnityEngine;
namespace NekoLegends
{
    public class GrassCulling
    {
        public Bounds m_bounds;

        public readonly List<GrassCulling> children = new List<GrassCulling>();
        public readonly List<int> grassIDHeld = new List<int>();

        public GrassCulling(Bounds bounds, int depth)
        {
            children.Clear();
            m_bounds = bounds;

            if (depth > 0)
            {
                Vector3 size = m_bounds.size;
                size /= 4.0f;
                Vector3 childSize = m_bounds.size / 2.0f;
                Vector3 center = m_bounds.center;


                // // layer 1
                Bounds topLeft = new Bounds(new Vector3(center.x - size.x, center.y - size.y, center.z - size.z), childSize);
                Bounds bottomRight = new Bounds(new Vector3(center.x + size.x, center.y - size.y, center.z + size.z), childSize);
                Bounds topRight = new Bounds(new Vector3(center.x - size.x, center.y - size.y, center.z + size.z), childSize);
                Bounds bottomLeft = new Bounds(new Vector3(center.x + size.x, center.y - size.y, center.z - size.z), childSize);

                // needs less subdiv in the y axis
                if (depth % 2 == 0)
                {
                    childSize.y = m_bounds.size.y;
                    children.Add(new GrassCulling(topLeft, depth - 1));
                    children.Add(new GrassCulling(bottomRight, depth - 1));
                    children.Add(new GrassCulling(topRight, depth - 1));
                    children.Add(new GrassCulling(bottomLeft, depth - 1));
                }
                else
                {
                    // // layer 2
                    Bounds topLeft2 = new Bounds(new Vector3(center.x - size.x, center.y + size.y, center.z - size.z), childSize);
                    Bounds bottomRight2 = new Bounds(new Vector3(center.x + size.x, center.y + size.y, center.z + size.z), childSize);
                    Bounds topRight2 = new Bounds(new Vector3(center.x - size.x, center.y + size.y, center.z + size.z), childSize);
                    Bounds bottomLeft2 = new Bounds(new Vector3(center.x + size.x, center.y + size.y, center.z - size.z), childSize);

                    children.Add(new GrassCulling(topLeft, depth - 1));
                    children.Add(new GrassCulling(bottomRight, depth - 1));
                    children.Add(new GrassCulling(topRight, depth - 1));
                    children.Add(new GrassCulling(bottomLeft, depth - 1));

                    children.Add(new GrassCulling(topLeft2, depth - 1));
                    children.Add(new GrassCulling(bottomRight2, depth - 1));
                    children.Add(new GrassCulling(topRight2, depth - 1));
                    children.Add(new GrassCulling(bottomLeft2, depth - 1));
                }
            }
        }

        public void RetrieveLeaves(Plane[] frustum, List<Bounds> list, List<int> visibleIDList)
        {
            if (GeometryUtility.TestPlanesAABB(frustum, m_bounds))
            {
                if (children.Count == 0)
                {
                    if (grassIDHeld.Count > 0)
                    {
                        list.Add(m_bounds);
                        visibleIDList.AddRange(grassIDHeld);
                    }
                }
                else
                {
                    foreach (GrassCulling child in children)
                    {
                        child.RetrieveLeaves(frustum, list, visibleIDList);
                    }
                }
            }
        }

        public bool FindLeaf(Vector3 point, int index)
        {
            bool FoundSpot = false;
            if (m_bounds.Contains(point))
            {
                if (children.Count != 0)
                {
                    foreach (GrassCulling child in children)
                    {
                        if (child.FindLeaf(point, index))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    grassIDHeld.Add(index);
                    return true;
                }
            }
            return FoundSpot;
        }

        public void RetrieveAllLeaves(List<GrassCulling> target)
        {
            if (children.Count == 0)
            {
                target.Add(this);
            }
            else
            {
                foreach (GrassCulling child in children)
                {
                    child.RetrieveAllLeaves(target);
                }
            }
        }

        public bool ClearEmpty()
        {
            bool delete = false;
            if (children.Count > 0)
            {
                //  DownSize();
                int i = children.Count - 1;
                while (i > 0)
                {
                    if (children[i].ClearEmpty())
                    {
                        children.RemoveAt(i);
                    }
                    i--;
                }
            }
            if (grassIDHeld.Count == 0 && children.Count == 0)
            {
                delete = true;
            }
            return delete;
        }
    }
}