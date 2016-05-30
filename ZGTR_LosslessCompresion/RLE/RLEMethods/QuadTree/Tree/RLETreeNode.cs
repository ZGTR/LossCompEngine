using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZGTR_LosslessCompresion.RLE.RLEMethods.QuadTree.Tree
{
    public class RLETreeNode
    {
        private RLETreeNode[] _childs;
        public RLETreeNode[] Childs
        {
            get { return _childs; }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
        }

        private bool _isArea;
        public bool IsArea
        {
            get { return _isArea; }
        }

        private int _i;
        public int IWidth
        {
            get { return _i; }
        }

        private int _j;
        public int JWidth
        {
            get { return _j; }
        }

        private int _subArrDim;
        public int SubArrDim
        {
            get { return _subArrDim; }
        }

        private int _level;
        public int Level
        {
            get { return _level; }
        }

        private Area _area;
        public Area Area
        {
            get { return _area; }
        }

        public RLETreeNode(Color color, bool isArea, int level, int i, int j, int subArrDim, Area area)
        {
            this._level = level;
            this._color = color;
            this._isArea = isArea;
            this._childs = new RLETreeNode[4];
            this._i = i;
            this._j = j;
            this._subArrDim = subArrDim;
            this._area = area;
            //for (int i = 0; i < 4; i++)
            //{
            //    this._childs[i] = new RLETreeNode(System.Drawing.Color.Empty, false, -1);
            //}
        }

    }
}
