
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxMediaPlayer_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		xie::CxStopwatch watch;

		// 1) Initialize.
		xie::Media::CxMediaPlayer controller;
		xie::CxString source_file = "images/src/stopwatch_320x240.wmv";
		xie::CxString output_file = "Results/CxMediaPlayer_01.avi";
		controller.Setup(source_file, NULL, output_file);
		
		#if defined(_MSC_VER)
		// DEBUG: The saved file can be confirmed in graphedt.
		{
			xie::CxString grf = "CxMediaPlayer_01.GRF";
			controller.SetParam("SaveGraphFile", grf);
		}
		#endif

		// 2) Periodic process.
		watch.Start();
		{
			auto duration = controller.GetDuration();
			controller.Start();					// Start.
			controller.WaitForCompletion(5000);	// Wait.
			controller.Stop();					// Stop.
		}
		watch.Stop();
		printf("%s: Elapsed=%f msec\n", __FUNCTION__, watch.Elapsed);
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): Code=%d Function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
