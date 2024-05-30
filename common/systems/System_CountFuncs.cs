using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Randomly{
    public class Vec2{
        public static Vector2 CurveType1(float lerp, Vector2[]points){
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