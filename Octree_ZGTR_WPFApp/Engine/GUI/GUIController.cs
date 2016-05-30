using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.MappingFunctions;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.AbstractOctree;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.Balanced;
using Octree_ZGTR_WPFApp.Engine.OctreeTree.OctreeTypes.UnBalanced;

namespace Octree_ZGTR_WPFApp.Engine.GUI
{
    public class GUIController
    {
        public static Octree GetOctreeType(MainWindow mainWindow, int colorTableDim)
        {
            Octree octree = null;
            if ((bool) mainWindow.rbc1.IsChecked)
            {
                octree = new OctreeBalancedLeastC7LCollapsing(colorTableDim);
            }
            else
            {
                if ((bool) mainWindow.rbc2.IsChecked)
                {
                    octree = new OctreeBalancedLeastCAllCollapsing(colorTableDim);
                }
                else
                {
                    if ((bool) mainWindow.rbc3.IsChecked)
                    {
                        octree = new OctreeBalancedLeastVAllSidesCollapsing(colorTableDim);
                    }
                    else
                    {
                        if ((bool) mainWindow.rbc4.IsChecked)
                        {
                            octree = new OctreeUnBalancedLeastChilds(colorTableDim);
                        }
                        else
                        {
                            if ((bool) mainWindow.rbc5.IsChecked)
                            {
                                octree = new OctreeUnBalancedLeastV1SideCollapsing(colorTableDim);
                            }
                            else
                            {
                                if ((bool) mainWindow.rbc6.IsChecked)
                                {
                                    octree = new OctreeUnBalancedLeastVAllSidesCollapsing(colorTableDim);
                                }
                            }
                        }
                    }
                }
            }
            return octree;
        }

        public static IMappingFunction GetMappingFuncitonType(MainWindow mainWindow)
        {
            IMappingFunction func = null;
            if ((bool)mainWindow.rbm1.IsChecked)
            {
                func = new ColorComponent(ColorComponentType.Red);
            }
            else
            {
                if ((bool)mainWindow.rbm2.IsChecked)
                {
                    func = new ColorComponent(ColorComponentType.Green);
                }
                else
                {
                    if ((bool)mainWindow.rbm3.IsChecked)
                    {
                        func = new ColorComponent(ColorComponentType.Blue);
                    }
                    else
                    {
                        if ((bool)mainWindow.rbm4.IsChecked)
                        {
                            func = new ColorComponentsSum();
                        }
                        else
                        {
                            if ((bool)mainWindow.rbm5.IsChecked)
                            {
                                func = new ColorWeightedComponentSum();
                            }
                            else
                            {
                                if ((bool)mainWindow.rbm6.IsChecked)
                                {
                                    func = new EcludianDistanceARGB();
                                }
                                else
                                {
                                    func = new EcludianDistanceColorComponents();
                                }
                            }
                        }
                    }
                }
            }
            return func;
        }

        //public static void MouseWheelEvent(MainWindow mainWindow, MouseWheelEventArgs e)
        //{
        //    try
        //    {
        //        if (mainWindow.imageInput.ActualWidth > 50)
        //        {
        //            if (e.Delta > 0)
        //                mainWindow.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
        //                                      new DoubleAnimation(mainWindow.imageInput.ActualWidth,
        //                                                          mainWindow.imageInput.ActualWidth + mainWindow.imageInput.ActualWidth / 2,
        //                                                          new Duration(TimeSpan.FromSeconds(1))));
        //            else
        //            {
        //                mainWindow.imageInput.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
        //                    new DoubleAnimation(mainWindow.imageInput.ActualWidth,
        //                 mainWindow.imageInput.ActualWidth - mainWindow.imageInput.ActualWidth / 2,
        //                 new Duration(TimeSpan.FromSeconds(1))));
        //            }

        //        }
        //        else
        //        {
        //            mainWindow.imageInput.BeginAnimation(System.Windows.Controls.Image.WidthProperty,
        //             new DoubleAnimation(mainWindow.imageInput.ActualWidth,
        //                                 mainWindow.imageInput.ActualWidth + mainWindow.imageInput.ActualWidth / 2,
        //                                 new Duration(TimeSpan.FromSeconds(1))));

        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}
    }
}
