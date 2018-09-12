/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

#pragma once

#ifndef _API_FILE_PLUGINS_H_INCLUDED_
#define _API_FILE_PLUGINS_H_INCLUDED_

#include "xie_high.h"

// ////////////////////////////////////////////////////////////
// PROTOTYPE

namespace xie
{
namespace File
{

// ////////////////////////////////////////////////////////////
// CONSTANT VARIABLE

const int XIE_FILE_OPENMODE_LOAD = 0;
const int XIE_FILE_OPENMODE_SAVE = 1;

// ////////////////////////////////////////////////////////////
// FUNCTION

void XIE_API fnPRV_File_Setup();
void XIE_API fnPRV_File_TearDown();

}
}

#endif
