
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxMonochrome_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/cube_320x240.png");
	xie::CxImage dst;

	// 1
	{
		xie::Effectors::CxMonochrome effector(1.0, 0.8, 0.7);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxMonochrome_01-1.png");
	}

	// 2
	{
		xie::Effectors::CxMonochrome effector(0.7, 1.0, 0.8);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxMonochrome_01-2.png");
	}

	// 3
	{
		xie::Effectors::CxMonochrome effector(0.7, 0.8, 1.0);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxMonochrome_01-3.png");
	}
}

}
