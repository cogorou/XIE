
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxPartColor_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png");
	xie::CxImage dst;

	// 1
	{
		xie::Effectors::CxPartColor effector(0, 0, 30);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxPartColor_01-1.png");
	}

	// 2
	{
		xie::Effectors::CxPartColor effector(0, 180, 60);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxPartColor_01-2.png");
	}

	// 3
	{
		xie::Effectors::CxPartColor effector(0, 180, 60, 0.114, 0.299, 0.587);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxPartColor_01-3.png");
	}
}

}
