using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZGTR_LosslessCompresion.RLE.RLEMethods.QuadTree.Tree;

namespace ZGTR_LosslessCompresion.RLE.RLEMethods.QuadTree
{
    public class RLEQuadTree
    {
        private Color[,] _colorArr;
        public Color[,] ColorArr
        {
            get { return _colorArr; }
        }

        private RLETreeNode _baseTreeNode;
        public RLETreeNode BaseNode
        {
            get { return _baseTreeNode; }
        }

        private int _arrDim;
        public int NodesCount { get; private set; }


        public RLEQuadTree(Color[,] colorArr)
        {
            this._colorArr = colorArr;
            this._arrDim = _colorArr.GetLength(0);
            this._baseTreeNode = new RLETreeNode(Color.Empty, true, 1, -1, -1, -1, Area.TopLeft);
        }

        public void BuildTree()
        {
            BuildQuadTreeRecursive(ref _baseTreeNode, 0, 0, _colorArr.GetLength(0) / 2, 1);
        }

        private void BuildQuadTreeRecursive(ref RLETreeNode currentBaseNode,int iWidth, int jHeight, double arrSubDim, int currentLevel)
        {
            try
            {
                NodesCount++;
                int intArrDim = (int)Math.Ceiling(arrSubDim);
                Color[,] currentSubArrTopLeft = GetSubArr(iWidth, jHeight, intArrDim, currentLevel, Area.TopLeft);
                Color[,] currentSubArrTopRight = GetSubArr(iWidth, jHeight, intArrDim, currentLevel, Area.TopRight);
                Color[,] currentSubArrBottomLeft = GetSubArr(iWidth, jHeight, intArrDim, currentLevel, Area.BottomLeft);
                Color[,] currentSubArrBottomRight = GetSubArr(iWidth, jHeight, intArrDim, currentLevel, Area.BottomRight);
                bool b1 = CheckAreaIsBulk(currentSubArrTopLeft);
                bool b2 = CheckAreaIsBulk(currentSubArrTopRight);
                bool b3 = CheckAreaIsBulk(currentSubArrBottomLeft);
                bool b4 = CheckAreaIsBulk(currentSubArrBottomRight);
                ManipulateChilds(ref currentBaseNode, b1, b2, b3, b4,
                    currentSubArrTopLeft[0, 0],
                    currentSubArrTopRight[0, 0],
                    currentSubArrBottomLeft[0, 0],
                    currentSubArrBottomRight[0, 0],
                    iWidth,
                    jHeight,
                    intArrDim);
                if (arrSubDim / 2 > 0)
                {
                    if (!b1)
                    {
                        BuildQuadTreeRecursive(ref currentBaseNode.Childs[0], iWidth, jHeight,
                                              arrSubDim / 2d, currentLevel + 1);
                    }
                    if (!b2)
                    {
                        BuildQuadTreeRecursive(ref currentBaseNode.Childs[1], iWidth + intArrDim, jHeight,
                                               arrSubDim / 2d, currentLevel + 1);
                    }
                    if (!b3)
                    {
                        BuildQuadTreeRecursive(ref currentBaseNode.Childs[2], iWidth, jHeight + intArrDim,
                                               arrSubDim / 2d, currentLevel + 1);
                    }
                    if (!b4)
                    {
                        BuildQuadTreeRecursive(ref currentBaseNode.Childs[3], iWidth + intArrDim, jHeight + intArrDim,
                                               arrSubDim / 2d, currentLevel + 1);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void ManipulateChilds(ref RLETreeNode currentBaseNode, bool b1, bool b2, bool b3, bool b4, Color color1, Color color2, Color color3,
            Color color4, int iWidth, int jHeight, int arrSubDim)
        {
            if (b1 == false)
            {
                currentBaseNode.Childs[0] = new RLETreeNode(Color.Empty, false, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.TopLeft);
            }
            else
            {
                currentBaseNode.Childs[0] = new RLETreeNode(color1, true, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.TopLeft);
            }
            if (b2 == false)
            {
                currentBaseNode.Childs[1] = new RLETreeNode(Color.Empty, false, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim ,Area.TopRight);
            }
            else
            {
                currentBaseNode.Childs[1] = new RLETreeNode(color2, true, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.TopRight);
            }
            if (b3 == false)
            {
                currentBaseNode.Childs[2] = new RLETreeNode(Color.Empty, false, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.BottomLeft);
            }
            else
            {
                currentBaseNode.Childs[2] = new RLETreeNode(color3, true, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.BottomLeft);
            }
            if (b4 == false)
            {
                currentBaseNode.Childs[3] = new RLETreeNode(Color.Empty, false, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.BottomRight);
            }
            else
            {
                currentBaseNode.Childs[3] = new RLETreeNode(color4, true, currentBaseNode.Level + 1, iWidth, jHeight, arrSubDim, Area.BottomRight);
            }
        }

        private bool CheckAreaIsBulk(Color[,] area)
        {
            Color colorToMatch = area[0, 0];
            bool isBulk = true;
            for (int i = 0; i < area.GetLength(0); i++)
            {
                for (int j = 0; j < area.GetLength(0); j++)
                {
                    if(area[j,i] != colorToMatch)
                    {
                        isBulk = false;
                        break;
                    }
                }
            }
            return isBulk;
        }

        private Color[,] GetSubArr(int iWidthStart, int jHeightStart, int arrSubDim, int currentLevel, Area areaWanted)
        {
            switch (areaWanted)
            {
                case Area.TopLeft:
                    return GetSubArr(iWidthStart,
                        iWidthStart + arrSubDim,
                        jHeightStart,
                        jHeightStart + arrSubDim,
                        arrSubDim);
                    break;
                case Area.TopRight:
                    return GetSubArr(iWidthStart + arrSubDim,
                        iWidthStart + arrSubDim * 2,
                        jHeightStart,
                        jHeightStart + arrSubDim,
                        arrSubDim);
                    break;
                case Area.BottomLeft:
                    return GetSubArr(iWidthStart,
                        iWidthStart + arrSubDim,
                        jHeightStart + arrSubDim,
                        jHeightStart + arrSubDim * 2,
                        arrSubDim);
                    break;
                case Area.BottomRight:
                    return GetSubArr(iWidthStart + arrSubDim,
                        iWidthStart + arrSubDim * 2,
                        jHeightStart + arrSubDim,
                        jHeightStart + arrSubDim * 2,
                        arrSubDim);
                    break;
            }
            // Will never reach here
            return null;
        }

        private void SetSubArr(int iWidthStart, int jHeightStart, int arrSubDim, int currentLevel, Area areaWanted, Color colorToFill, Color[,] colorArr)
        {
            switch (areaWanted)
            {
                case Area.TopLeft:
                    SetSubArr(iWidthStart,
                        iWidthStart + arrSubDim,
                        jHeightStart,
                        jHeightStart + arrSubDim,
                        arrSubDim,
                        colorToFill,
                        colorArr);
                    break;
                case Area.TopRight:
                    SetSubArr(iWidthStart + arrSubDim,
                        iWidthStart + arrSubDim * 2,
                        jHeightStart,
                        jHeightStart + arrSubDim,
                        arrSubDim,
                        colorToFill,
                        colorArr);
                    break;
                case Area.BottomLeft:
                    SetSubArr(iWidthStart,
                        iWidthStart + arrSubDim,
                        jHeightStart + arrSubDim,
                        jHeightStart + arrSubDim * 2,
                        arrSubDim,
                        colorToFill,
                        colorArr);
                    break;
                case Area.BottomRight:
                    SetSubArr(iWidthStart + arrSubDim,
                        iWidthStart + arrSubDim * 2,
                        jHeightStart + arrSubDim,
                        jHeightStart + arrSubDim * 2,
                        arrSubDim,
                        colorToFill,
                        colorArr);
                    break;
            }
        }

        private Color[,] GetSubArr(int iStart, int iEnd, int jStart, int jEnd, int width)
        {
            Color[,] arrToReturn = new Color[width, width];
            int iInner = 0;
            int jInner = 0;
            if (iEnd > _colorArr.GetLength(1))
            {
                iEnd = _colorArr.GetLength(1);
            }
            if (jEnd > _colorArr.GetLength(0))
            {
                jEnd = _colorArr.GetLength(0);
            }
            for (int i = iStart; i < iEnd; i++)
            {
                for (int j = jStart; j < jEnd; j++)
                {
                    if (j == 0 && i == 5)
                    {
                        int bn = 3;
                    }
                    arrToReturn[jInner, iInner] = _colorArr[j, i];
                    jInner++;
                }
                jInner = 0;
                iInner++;
            }
            return arrToReturn;
        }

        private void SetSubArr(int iStart, int iEnd, int jStart, int jEnd, int width, Color colorToFill, Color[,] colorArr)
        {
            try
            {
                if (iEnd > _colorArr.GetLength(1))
                {
                    iEnd = _colorArr.GetLength(1);
                }
                if (jEnd > _colorArr.GetLength(0))
                {
                    jEnd = _colorArr.GetLength(0);
                }
                for (int i = iStart; i < iEnd; i++)
                {
                    for (int j = jStart; j < jEnd; j++)
                    {
                        colorArr[j, i] = colorToFill;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public Color[,] RetriveColorArrFromQuadTree()
        {
            Color[,] colorArrToReturn = new Color[_arrDim, _arrDim];
            InfiltrateTree(_baseTreeNode, ref colorArrToReturn);
            return colorArrToReturn;
        }

        public void InfiltrateTree(RLETreeNode currentNode, ref Color[,] colorArrRetieved)
        {
            try
            {
                foreach (RLETreeNode nodeChild in currentNode.Childs)
                {
                    if (nodeChild.IsArea)
                    {
                        FillArea(nodeChild, colorArrRetieved);
                    }
                    else
                    {
                        if (nodeChild.Childs[0] == null)
                        {
                            FillArea(nodeChild, colorArrRetieved);
                        }
                        else
                        {
                            InfiltrateTree(nodeChild, ref colorArrRetieved);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void FillArea(RLETreeNode nodeChild, Color[,] colorArrRetieved)
        {
            int iStart = nodeChild.IWidth;
            int jStart = nodeChild.JWidth;
            SetSubArr(iStart, jStart, nodeChild.SubArrDim, nodeChild.Level, nodeChild.Area,
                      nodeChild.Color, colorArrRetieved);
        }
    }
}