using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
  public static class ImageHelper
  {

    public static Image resizeImg(string ImagePath, int width)
    {

      Image img = Image.FromFile(ImagePath, true);

      while(img == null)
      {
        img = Image.FromFile(ImagePath, true);
      }

      // 4:3 Aspect Ratio. You can also add it as parameters
      double aspectRatio_X = 1000;
      double aspectRatio_Y = 346;
      double targetHeight = Convert.ToDouble(width) / (aspectRatio_X / aspectRatio_Y);

      img = cropImg(img);
      Bitmap bmp = new Bitmap(width, (int)targetHeight);
      Graphics grp = Graphics.FromImage(bmp);
      if (img != null)
      {
        grp.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
      }

      return (Image)bmp;

    }

    private static Image cropImg(Image img)
    {

      if (img.Width > 1000 || img.Height > 346)
      {
        double imgWidth = Convert.ToDouble(img.Width);
        double imgHeight = Convert.ToDouble(img.Height);

        double extraWidth = imgWidth - 1000;
        double cropStartFrom = extraWidth;

        Bitmap bmp = new Bitmap((int)(1000), 346);
        Graphics grp = Graphics.FromImage(bmp);
        grp.DrawImage(img, new Rectangle(0, 0, (int)(1000), 346), new Rectangle((int)cropStartFrom, 0, (int)(imgWidth - extraWidth), img.Height), GraphicsUnit.Pixel);
        return (Image)bmp;

      }
      else
        return null;
    }
  }
}