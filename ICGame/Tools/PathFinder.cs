﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Point=System.Drawing.Point;
using System.Threading;

namespace ICGame
{
    class PathFinder
    {
        private const int M = 10000;
        /*
                private const int UNAVALIABLE = 0;
                private int[,] graph;
        */
        private static int[,] map;
        private static bool initialized;
        private int[,] lastProcessed;
        private int lastRadius = -1;
        private List<Point> path;
        private DateTime pathTime;

        public PathFinder(int[,] map, List<GameObject> gameObjects)
        {
            PathFinder.map = PutObjects(ref map, gameObjects);
            initialized = true;
        }

        /// <summary>
        /// Avaliable only if initialized before.
        /// </summary>
        public PathFinder()
        {
            if(!initialized)
                throw new MissingFieldException("PathFinder was not initialized");
        }

        public void FindPath(Point start, Point goal, int objectRadius, IControllable controllable)
        {
            if(objectRadius != lastRadius)
            {
                lastProcessed = ProcessMap(map, objectRadius);
                lastRadius = objectRadius;
            }
            Thread th = new Thread(new ParameterizedThreadStart(AStar));
            pathTime = DateTime.Now;
            th.Start(new AStarArg(start,goal,controllable));
            //AStar(start, goal, out path);
        }

        private static int[,] ProcessMap(int[,] map, int objectRadius)
        {
            objectRadius = objectRadius;
            int[,] resultMap = new int[map.GetLength(0),map.GetLength(1)];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    resultMap[i, j] = map[i, j];
                }
            }

            for (int i = 0; i < map.GetLength(0); ++i )
            {
                for(int j = 0; j < map.GetLength(1); ++j)
                {
                    if (map[i, j] != 1)
                    {
                        for (int k = i - objectRadius; k <= i + objectRadius; ++k)
                        {
                            if (k >= 0 && k < map.GetLength(0))
                            {
                                for (int l = j - objectRadius; l <= j + objectRadius; ++l)
                                {
                                    if (l >= 0 && l < map.GetLength(1))
                                    {
                                        resultMap[k, l] = resultMap[k, l] > map[i, j] ? resultMap[k, l] : map[i, j];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        resultMap[i, j] = resultMap[i, j] > 1 ? resultMap[i, j] : 1;
                    }
                }
            }

            return resultMap;
        }

        private static int[,] PutObjects(ref int[,] map, List<GameObject> gameObjects)
        {
            int[,] output = new int[map.GetLength(0),map.GetLength(1)];
            //map.CopyTo(output, 0);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    output[i, j] = map[i, j];
                }
            }

            foreach (GameObject gameObject in gameObjects)
            {
                if(gameObject is Building || gameObject is StaticObject)
                {
                    IPhysical physical = gameObject as IPhysical;
                    for(int i = Convert.ToInt32(Math.Floor(gameObject.Position.X - physical.Width/2.0)); 
                        i<Convert.ToInt32(Math.Ceiling(gameObject.Position.X + physical.Width/2.0)); ++i)
                    {
                        for(int j = Convert.ToInt32(Math.Floor(gameObject.Position.Z - physical.Length/2.0)); 
                            j<Convert.ToInt32(Math.Ceiling(gameObject.Position.Z + physical.Length/2.0)); ++j)
                        {
                            if(i<0 || j < 0)
                                throw new ArgumentOutOfRangeException("Bad location of object" + physical.ToString());
                            output[i, j] = M;
                        }
                    }
                }
            }

            return output;
        }


        internal class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Value { get; set; }
            public double GScore { get; set; }
            public double HScore { get; set; }
            public double FScore { get; set; }
            public int CameFrom { get; set; }

            public Node(int x, int y, int value)
            {
                X = x;
                Y = y;
                Value = value;
                GScore = 0;
                HScore = 0;
                FScore = 0;
                CameFrom = -1;
            }

            public override bool Equals(object obj)
            {
 	            if(obj is Node)
 	            {
 	                Node nd = obj as Node;
                    if(X == nd.X && Y == nd.Y)
                    {
                        return true;
                    }
 	            }
 	            return false;
            }
        }

        public void TestNodes()
        {
            List<Node> l = new List<Node>();

            Node n = new Node(1,2,3) {HScore = 2};

            l.Add(n);

            Node d = new Node(4,5,6) {HScore = 2};

            MessageBox.Show(Convert.ToString(l.Contains(d)));

            d = new Node(1,2,6);

            d.HScore = 2;

            MessageBox.Show(Convert.ToString(l.Contains(d)));

            d = new Node(1,2,5) {HScore = 54};

            MessageBox.Show(Convert.ToString(l.Contains(d)));
        }

        private static List<Node> GetNeighbors(Node pt, int[,] map)
        {
            List<Node> nbrs = new List<Node>();
            if(pt.X > 0)
            {
                if(pt.Y > 0)
                {
                    nbrs.Add(new Node(pt.X-1, pt.Y-1, map[pt.X-1, pt.Y-1]));
                }
                nbrs.Add(new Node(pt.X - 1, pt.Y, map[pt.X - 1, pt.Y]));
                if (pt.Y < map.GetLength(1) - 1)
                {
                    nbrs.Add(new Node(pt.X - 1, pt.Y + 1, map[pt.X - 1, pt.Y + 1]));
                }
            }
            if (pt.Y > 0)
            {
                nbrs.Add(new Node(pt.X, pt.Y - 1, map[pt.X, pt.Y - 1]));
            }
            if (pt.Y < map.GetLength(1) - 1)
            {
                nbrs.Add(new Node(pt.X, pt.Y + 1, map[pt.X, pt.Y + 1]));
            }
            if(pt.X < map.GetLength(0) - 1)
            {
                if (pt.Y > 0)
                {
                    nbrs.Add(new Node(pt.X + 1, pt.Y - 1, map[pt.X + 1, pt.Y - 1]));
                }
                nbrs.Add(new Node(pt.X + 1, pt.Y, map[pt.X + 1, pt.Y]));
                if (pt.Y < map.GetLength(1) - 1)
                {
                    nbrs.Add(new Node(pt.X + 1, pt.Y + 1, map[pt.X + 1, pt.Y + 1]));
                }
            }
            return nbrs;
        }

        private static double EstimateDistance(Node start, Point goal)
        {
            int xDist = Math.Abs(start.X - goal.X);
            int yDist = Math.Abs(start.Y - goal.Y);

            int sqDist = xDist < yDist ? xDist : yDist;

            return Math.Sqrt(2)*sqDist + (double)(xDist - sqDist) + (double)(yDist - sqDist);
        }

        private static int GetClosest(List<Node> queue)
        {
            if(queue.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Queue is empty");
            }
            double closest = Double.MaxValue;
            int index = -1;

            for (int i = 0; i < queue.Count; ++i)
            {
                if(queue[i].FScore < closest)
                {
                    closest = queue[i].FScore;
                    index = i;
                }
                if(closest == 0)
                {
                    return index;
                }
            }

            return index;
        }

        private static List<Point> ReconstructPath(Node final, List<Node> nodes)
        {
            List<Point> path = new List<Point>();

            Node cur = final;

            path.Add(new Point(cur.X, cur.Y));

            while (cur.CameFrom != -1)
            {
                cur = nodes[cur.CameFrom];
                path.Insert(0,new Point(cur.X, cur.Y));
            }

            return path;
        }

        private void AStar(Object obj)
        {
            AStarArg aStarArg = (AStarArg) obj;
            AStar(aStarArg.Start, aStarArg.Goal, out path);
            if(pathTime > aStarArg.Controllable.PathTime)
            {
                this.OpitmizePath(ref path);
                aStarArg.Controllable.Path = path;
                aStarArg.Controllable.NextStep = new Vector2(((GameObject) aStarArg.Controllable).Position.X,((GameObject) aStarArg.Controllable).Position.Z);
                aStarArg.Controllable.PathTime = pathTime;
            }
            /*string msg = "";
            for(int i = 0; i < path.Count; ++i)
            {
                msg += path[i].ToString() + " " + lastProcessed[path[i].Y, path[i].X].ToString() + " " +
                       map[path[i].Y, path[i].X].ToString() + "  |  ";
            }
            MessageBox.Show(msg);*/
        }

        private void OpitmizePath(ref List<Point> path)
        {
            int i = 0;
            while(i < path.Count-2)
            {
                int difY1 = path[i + 1].Y - path[i].Y;
                int difY2 = path[i + 2].Y - path[i + 1].Y;
                if ((difY1 == 0 && difY2 == 0) || (difY1 != 0 && difY2 != 0 && (path[i + 1].X - path[i].X) / difY1 == (path[i + 2].X - path[i + 1].X) / difY2))
                {
                    path.RemoveAt(i+1);
                }
                else if (lastProcessed[path[i].X, path[i].Y] == lastProcessed[path[i+1].X, path[i+1].Y] && 
                    lastProcessed[path[i].X, path[i].Y] == lastProcessed[path[i+2].X, path[i+2].Y])
                {
                    bool remove = true;

                    int val = lastProcessed[path[i].X, path[i].Y];
                    int minX = path[i].X < path[i + 2].X ? path[i].X : path[i+2].X;
                    int maxX = path[i].X > path[i + 2].X ? path[i].X : path[i+2].X;
                    int minY = path[i].Y < path[i + 2].Y ? path[i].Y : path[i+2].Y;
                    int maxY = path[i].Y > path[i + 2].Y ? path[i].Y : path[i+2].Y;
                    for(int k = minX; k < maxX; ++k)
                    {
                        for (int l = minY; l < maxY; l++)
                        {
                            if(lastProcessed[k,l] != val)
                            {
                                remove = false;
                            }
                        }
                    }

                    if(remove)
                    {
                        path.RemoveAt(i+1);
                    }
                    else
                    {
                        ++i;
                    }
                }
                else
                {
                    ++i;
                }
            }
        }

        private void AStar(Point start, Point goal, out List<Point> path)
        {
            path = new List<Point>();

            List<Node> queue = new List<Node>();
            List<Node> done = new List<Node>();

            queue.Add(new Node(start.X,start.Y,lastProcessed[start.X, start.Y]));
            queue[0].GScore = 0;
            queue[0].CameFrom = -1;

            while (queue.Count > 0)
            {
                int index = GetClosest(queue);
                Node cur = queue[index];

                if(cur.X == goal.X && cur.Y == goal.Y)
                {
                    path = ReconstructPath(cur, done);
                    return;
                }

                queue.RemoveAt(index);
                done.Add(cur);

                foreach (Node node in GetNeighbors(cur, lastProcessed))
                {
                    if(!done.Contains(node))
                    {
                        double diag = 1;
                        if(Math.Abs(cur.X - node.X) + Math.Abs(cur.Y - node.Y) == 2)
                        {
                            diag = Math.Sqrt(2);
                        }
                        double gScore = cur.GScore + diag*(node.Value + cur.Value)/2.0;
                        bool isBetter = false;
                        if(!queue.Contains(node))
                        {
                            node.HScore = EstimateDistance(node, goal);
                            queue.Add(node);
                            isBetter = true;
                        }
                        else if(gScore < node.GScore)
                        {
                            isBetter = true;
                        }
                        if(isBetter)
                        {
                            node.CameFrom = done.Count - 1;
                            node.GScore = gScore;
                            node.FScore = node.GScore + node.HScore;
                        }
                    }
                }
            }
            
        }

        internal class AStarArg
        {
            public Point Start{ get; set; }
            public Point Goal { get; set; }
            public IControllable Controllable { get; set; }

            public AStarArg(Point start, Point goal, IControllable controllable)
            {
                Start = start;
                Goal = goal;
                Controllable = controllable;
            }
        }

    }
}
