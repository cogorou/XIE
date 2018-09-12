/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _CXGLCONTEXT_H_INCLUDED_
#define _CXGLCONTEXT_H_INCLUDED_

namespace xie
{
namespace GDI
{

// ============================================================
template<class TB> class CxGLContext
{
public:
	TB*		Body;
	bool	UseFB;

	CxGLContext(TB* body, bool useFB)
	{
		Body = body;
		UseFB = useFB;
		Enter();
	}
	~ CxGLContext()
	{
		Leave();
	}
	void Enter()
	{
		Body->BeginPaint();
		if (UseFB)
			Body->Lock();
	}
	void Leave()
	{
		if (UseFB)
			Body->Unlock();
		Body->EndPaint();
	}
};

}
}

#endif
