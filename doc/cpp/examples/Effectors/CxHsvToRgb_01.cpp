
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxHsvToRgb_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png", true);	// true=unpack
	xie::CxImage hsv;
	xie::CxImage rgb;

	// RGB to HSV
	{
		xie::Effectors::CxRgbToHsv effector(0);
		effector.Execute(src, hsv);

		// modify saturation
		{
			auto sat = hsv.Child(1);
			sat.Filter().Mul(sat, 0.5);
		}

		// for DEBUG
		{
			auto hue = hsv.Child(0);
			auto sat = hsv.Child(1);
			auto val = hsv.Child(2);

			hue.Depth(1);
			hue.Save("Results/CxHsvToRgb_01-HSV0.raw");

			sat.Depth(1);
			sat.Save("Results/CxHsvToRgb_01-HSV1.raw");

			val.Depth(1);
			val.Save("Results/CxHsvToRgb_01-HSV2.raw");
		}
	}

	// HSV to RGB
	{
		xie::Effectors::CxHsvToRgb effector(0);
		effector.Execute(hsv, rgb);
		// for DEBUG
		rgb.Save("Results/CxHsvToRgb_01-RGB.png");
	}
}

}
