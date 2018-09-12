
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>

namespace User
{

// ============================================================
void CxScreenCapture_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		xie::CxStopwatch watch;

		// 1) get screen list.
		xie::Media::CxScreenList list;
		list.Setup();

		// 2) Initialize.
		xie::Media::CxScreenListItem window = list[0];
		xie::Media::CxDeviceParam audioParam(NULL, 0);
		xie::CxString output_file = "Results/CxScreenCapture_01.avi";
		xie::Media::CxScreenCapture controller;
		controller.Setup(window, audioParam, output_file);

		#if defined(_MSC_VER)
		// DEBUG: The saved file can be confirmed in graphedt.
		{
			xie::CxString grf = "CxScreenCapture_01.GRF";
			controller.SetParam("SaveGraphFile", grf);
		}
		#endif

		// 3) Periodic process.
		watch.Start();
		{
			auto grabber = controller.CreateGrabber();
			grabber.Notify = xie::Media::CxGrabberEvent(
				[](void* sender, xie::Media::CxGrabberArgs* e)
				{
				});
			grabber.Reset();
			grabber.Start();

			// Start capturing.
			controller.Start();

			// Wait until the stop or the specified time has elapsed.
			if (grabber.Wait(5000) == false)
				grabber.Stop();

			// Stop capturing.
			controller.Stop();
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
