
#include "xie_core.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxImage_CalcDepth_01()
{
	printf("%s\n", __FUNCTION__);

	xie::CxImage src("images/src/starflower_320x240.jpg");
	xie::CxImage dst;

	// pre
	int depth_pre = dst.CalcDepth(-1);
	printf("%-20s = %d\n", "dst.CalcDepth(-1)", depth_pre);
	printf("%-20s = %d\n", "dst.Depth", dst.Depth());

	// post
	dst.Filter().Div(src, 2);
	int depth_post = dst.CalcDepth(-1);
	dst.Depth(depth_post);
	printf("%-20s = %d\n", "dst.CalcDepth(-1)", depth_post);
	printf("%-20s = %d\n", "dst.Depth", dst.Depth());
}

}
