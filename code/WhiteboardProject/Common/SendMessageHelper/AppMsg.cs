using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteboardProject.Common
{
    public enum AppMsg
    {
        Highlighter,//荧光笔
        Eraser,//橡皮擦
        Hardpen,//画笔
        WritingBrush,//毛笔
        Seal,//印章
        Softpen,//软笔
        SelectErase,//选择擦除
        PointErase,//点擦除
        ClearErase,//清空擦除
        GestureErase,//手势擦除
        ShapeChanged,//图形改变
        ColorChanged,//颜色改变
        CloseCommand,//关闭消息
        SliderValueChanged,//漫游缩放值
        BottomLeftNavigation,//左下角消息通知
        ExportFile,//导出
    }

    public enum Shape
    {
        rectangle,//长方形
        round,//圆形
        oval,//椭圆
        triangle,//三角形
        trapezoid,//梯形
        square,//正方形
        pentagon,//正五边形
        hexagon,//正六边形
        linesegment,//线段
        arrow,//方向箭头
        dottedline,//虚线段
        polyline,//折线段
        pointdownward,//向下指的图形
        bucket,//颜料桶
    }

    public enum SealShapeEnum
    {
        Love,//爱心
        MapleLeaf,//枫叶
        Smiley,//笑脸
        Sun,//太阳
        Star,//星星
    }
    }
