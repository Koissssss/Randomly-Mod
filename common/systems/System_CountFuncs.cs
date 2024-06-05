using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Randomly{
    public class Cnt{
        public static int Div(float num1, float num2){
            int k;
            k = (int)(Math.Abs(num1) / num2) + 1;
            num1 += k * num2;
            return (int)(num1 / num2) - k;
        }
    }
    public class Vec2{
        
        /// <summary>
        /// 返回一个边数为<c>Pnum</c>，顺时针旋转<c>baseRotation</c>角度的多边形上的一点，
        /// <br/>其中<c>Dnum</c>每条边上的点数（两个顶点算作一个点），<c>Index</c>代表目标点是所有点中第几个，
        /// <br/><c>r</c>代表顶点到多边形中心的距离
        /// </summary>
        public static Vector2 Polygon_Type1(float baseRotation, int Pnum, int Dnum, float Index, float r){
            int lineIndex = Cnt.Div(Index, Dnum);
            float lineLerp = Index / Dnum - lineIndex;
            float perRot = (float)(2 * Math.PI / Pnum);
            Vector2 rVec2 = new Vector2(r, 0);
            Vector2 BaseVec2 = Vector2.Lerp(rVec2, rVec2.RotatedBy(perRot), lineLerp);
            return BaseVec2.RotatedBy(baseRotation * Math.PI / 180 + lineIndex * perRot);
        }
        /// <summary>
        /// 产生一个按顺序经过所有<c>points</c>点的贝塞尔曲线，
        /// <br/>其中<c>lerp = 0</c>代表曲线起点，<c>lerp = 1</c>代表曲线终点.
        /// </summary>
        public static Vector2 CurveType1_Bezier(float lerp, Vector2[]points){
            if(lerp < 0)lerp = 0; if(lerp > 1)lerp = 1;
            float midlerp = lerp * (points.Length - 1) % 1;
            int index = (int)(lerp * (points.Length - 1));
            if(index == points.Length - 1){
                return points[points.Length - 1];
            }
            Vector2 ptl = points[index];
            Vector2 ptr = points[index + 1];
            Vector2 ptlr;
            Vector2 ptrl;
            if(index == 0){
                float l1, l2, l3;
                float length;
                l1 = (points[index + 1] - points[index]).Length();
                l2 = (points[index + 2] - points[index + 1]).Length();
                l3 = (points[index + 2] - points[index]).Length();
                if(l1 == 0 || l2 == 0 || l3 == 0)length = 0;
                else length = 1/(1 / l1 + 1 / l2 + 1 / l3); 
                ptlr = ptrl = ptr - length * (points[index + 2] - points[index]).SafeNormalize(Vector2.Zero);
            }
            else if(index == points.Length - 2){
                float l1, l2, l3;
                float length;
                l1 = (points[index] - points[index - 1]).Length();
                l2 = (points[index + 1] - points[index]).Length();
                l3 = (points[index + 1] - points[index - 1]).Length();
                if(l1 == 0 || l2 == 0 || l3 == 0)length = 0;
                else length = 1/(1 / l1 + 1 / l2 + 1 / l3); 
                ptrl = ptlr = ptl + length * (points[index + 1] - points[index - 1]).SafeNormalize(Vector2.Zero);
            }
            else{
                float l1, l2, l3;
                float length;
                l1 = (points[index] - points[index - 1]).Length();
                l2 = (points[index + 1] - points[index]).Length();
                l3 = (points[index + 1] - points[index - 1]).Length();
                if(l1 == 0 || l2 == 0 || l3 == 0)length = 0;
                else length = 1/(1 / l1 + 1 / l2 + 1 / l3); 
                ptlr = ptl + length * (points[index + 1] - points[index - 1]).SafeNormalize(Vector2.Zero);
                l1 = (points[index + 1] - points[index]).Length();
                l2 = (points[index + 2] - points[index + 1]).Length();
                l3 = (points[index + 2] - points[index]).Length();
                if(l1 == 0 || l2 == 0 || l3 == 0)length = 0;
                else length = 1/(1 / l1 + 1 / l2 + 1 / l3); 
                ptrl = ptr - length * (points[index + 2] - points[index]).SafeNormalize(Vector2.Zero);
            }
            Vector2 midpt1 = Vector2.Lerp(ptl, ptlr, midlerp);
            Vector2 midpt2 = Vector2.Lerp(ptlr, ptrl, midlerp);
            Vector2 midpt3 = Vector2.Lerp(ptrl, ptr, midlerp);
            Vector2 mmidpt1 = Vector2.Lerp(midpt1, midpt2, midlerp);
            Vector2 mmidpt2 = Vector2.Lerp(midpt2, midpt3, midlerp);
            return Vector2.Lerp(mmidpt1, mmidpt2, midlerp);
        }


        public static Vector2 LengthdirDeg(float length, float dir) => LengthdirDeg((double)length, (double)dir);
        public static Vector2 LengthdirDeg(double length, float dir) => LengthdirDeg(length, (double)dir);
        public static Vector2 LengthdirDeg(float length, double dir) => LengthdirDeg((double)length, dir);
        public static Vector2 LengthdirDeg(double length, double dir){
            double dirRad = dir * Math.PI / 180;
            return new((float)(Math.Cos(dirRad) * length), (float)(Math.Sin(dirRad) * length));
        }
        public static Vector2 LengthdirRad(float length, float dir) => LengthdirRad((double)length, (double)dir);
        public static Vector2 LengthdirRad(double length, float dir) => LengthdirRad(length, (double)dir);
        public static Vector2 LengthdirRad(float length, double dir) => LengthdirRad((double)length, dir);
        public static Vector2 LengthdirRad(double length, double dir){
            return new((float)(Math.Cos(dir) * length), (float)(Math.Sin(dir) * length));
        }
    }
}