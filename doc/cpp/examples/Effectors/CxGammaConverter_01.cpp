
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxGammaConverter_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/starflower_320x240.jpg");
	xie::CxImage dst;

	// 1
	{
		xie::Effectors::CxGammaConverter effector(0, 2.0);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxGammaConverter_01-1.jpg");
	}

	// 2
	{
		xie::Effectors::CxGammaConverter effector(0, 0.75);
		effector.Execute(src, dst);
		// for DEBUG
		dst.Save("Results/CxGammaConverter_01-2.jpg");
	}
}

}
