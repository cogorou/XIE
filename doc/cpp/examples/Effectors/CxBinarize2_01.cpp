
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxBinarize2_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/frog_320x240_u8x1.png");
	xie::CxImage dst;
	xie::Effectors::CxBinarize2 effector(xie::TxRangeD(128, 164), false, xie::TxRangeD(0, 1));
	effector.Execute(src, dst);

	// for DEBUG
	dst.Depth(1);
	dst.Save("Results/CxBinarize2_01.png");
}

}
