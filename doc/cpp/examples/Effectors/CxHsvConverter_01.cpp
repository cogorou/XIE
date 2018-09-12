
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxHsvConverter_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png");
	xie::CxImage dst;

	// hue
	{
		xie::Effectors::CxHsvConverter effector(0, 180, 1.0, 1.0);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxHsvConverter_01-1.png");
	}

	// saturation
	{
		xie::Effectors::CxHsvConverter effector(0, 0, 0.5, 1.0);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxHsvConverter_01-2.png");
	}

	// value
	{
		xie::Effectors::CxHsvConverter effector(0, 0, 1.0, 0.5);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxHsvConverter_01-3.png");
	}
}

}
