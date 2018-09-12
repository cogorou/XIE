
#include "xie_core.h"
#include "xie_high.h"
#include <stdio.h>
#include <vector>

namespace User
{

// ============================================================
void CxCamera_01()
{
	printf("%s\n", __FUNCTION__);

	try
	{
		xie::CxStopwatch watch;

		xie::Media::CxCamera controller;

		// Setup
		xie::Media::CxDeviceParam video(NULL, 0, 0, {640, 480});
		xie::Media::CxDeviceParam audio(NULL, 0);
		xie::CxStringA outputFile = "Results/CxCamera_01.wmv"; 
		controller.Setup(video, audio, outputFile);

		// Start exposure
		controller.Start();

		// Grabber
		std::vector<xie::Media::CxGrabberArgs> args;
		auto grabber = controller.CreateGrabber(xie::Media::ExMediaType::Video);
		grabber.Notify = xie::Media::CxGrabberEvent(
			[&args](void* sender, xie::Media::CxGrabberArgs* e)
			{
				args.push_back(*e);
			});
		grabber.Reset();
		grabber.Start();
		if (grabber.Wait(1000) == false)
			grabber.Stop();

		// Stop exposure
		controller.Stop();

		// dump
		printf("args.size()=%d\n", args.size());
		for(int i=0 ; i<(int)args.size() ; i++)
		{
			auto dt = xie::TxDateTime::FromBinary(args[i].TimeStamp(), true);
			printf("args[%d]= %d %02d:%02d:%02d.%03d\n",
				i,
				args[i].Index(),
				dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
		}
	}
	catch(const xie::CxException& ex)
	{
		printf("%s(%d): code=%d function=%s\n", ex.File(), ex.Line(), ex.Code(), ex.Function());
	}
}

}
