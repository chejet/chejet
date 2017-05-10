using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace XkCms.Common.Utils
{
    /// <summary>
    /// ö��,��������ͼģʽ
    /// </summary>
    public enum ThumbnailMod : byte { HW, W, H, Cut };

    public class PicDeal
    {
        public static bool MakeThumbnail(string originalImagePath, int width, int height, ThumbnailMod mode)
        {
            string thumbnailPath = originalImagePath.Substring(0, originalImagePath.LastIndexOf('.')) + "s.jpg";
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumbnailMod.HW://ָ���߿����ţ����ܱ��Σ�                
                    break;
                case ThumbnailMod.W://ָ�����߰�����                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailMod.H://ָ���ߣ�������
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailMod.Cut://ָ���߿�ü��������Σ�                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //�½�һ��bmpͼƬ
            Image bitmap = new Bitmap(towidth, toheight);

            //�½�һ������
            Graphics g = Graphics.FromImage(bitmap);

            //���ø�������ֵ��
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //���ø�����,���ٶȳ���ƽ���̶�
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //��ջ�������͸������ɫ���
            g.Clear(System.Drawing.Color.Transparent);

            //��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);
            bool isok = false;
            try
            {
                //��jpg��ʽ��������ͼ
                bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
                isok = true;
            }
            catch (Exception)
            {
                thumbnailPath = originalImagePath;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return isok;
        }

        /// <summary>
        /// ��ͼƬ������ͼƬˮӡ
        /// </summary>
        /// <param name="Path">ԭ������ͼƬ·��</param>
        /// <param name="Path_syp">���ɵĴ�ͼƬˮӡ��ͼƬ·��</param>
        /// <param name="Path_sypf">ˮӡͼƬ·��</param>
        public static void AddWaterPic(string Path, string Path_sypf)
        {
            try
            {
                Image image = Image.FromFile(Path);
                Image copyImage = Image.FromFile(Path_sypf);
                Graphics g = Graphics.FromImage(image);
                g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
                g.Dispose();

                image.Save(Path + ".temp");
                image.Dispose();
                System.IO.File.Delete(Path);
                File.Move(Path + ".temp", Path);
            }
            catch
            { }
        }
    }
}
