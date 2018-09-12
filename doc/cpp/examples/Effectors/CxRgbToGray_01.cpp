
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxRgbToGray_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png");
	xie::CxImage dst;

	// 1
	{
		xie::Effectors::CxRgbToGray effector(0, 0.299, 0.587, 0.114);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxRgbToGray_01-1.png");
	}

	// 2
	{
		xie::Effectors::CxRgbToGray effector(0, 0.114, 0.299, 0.587);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxRgbToGray_01-2.png");
	}
}

}
