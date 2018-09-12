
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxRgbToHsv_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png", true);	// true=unpack
	xie::CxImage dst;

	xie::Effectors::CxRgbToHsv effector(0);
	effector.Execute(src, dst);

	// for DEBUG
	{
		auto hue = dst.Child(0);
		auto sat = dst.Child(1);
		auto val = dst.Child(2);

		hue.Depth(1);
		hue.Save("Results/CxRgbToHsv_01-0.raw");

		sat.Depth(1);
		sat.Save("Results/CxRgbToHsv_01-1.raw");

		val.Depth(1);
		val.Save("Results/CxRgbToHsv_01-2.raw");
	}
}

}
