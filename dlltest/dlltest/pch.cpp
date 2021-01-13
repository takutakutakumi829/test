// pch.cpp: プリコンパイル済みヘッダーに対応するソース ファイル

#include "pch.h"

DLLEXPORT bool __stdcall CheckFaceRect(cv::Rect rect)
{
	if (rect.x == 0 && rect.y == 0 && rect.width == 0 && rect.height == 0)
	{
		return true;
	}
	return false;
}

DLLEXPORT bool __stdcall CheckFaceSize(std::vector<cv::Rect> faces)
{
	return faces.size() == 0;
}

DLLEXPORT bool __stdcall CheckMat(cv::Mat mat)
{
	return !mat.empty();
}

DLLEXPORT bool __stdcall CheckBasicPos(cv::Point point)
{
	return (point.x == 0 && point.y == 0);
}

DLLEXPORT bool __stdcall CheckFaceFlag(int flag)
{
	return flag == 0;
}

DLLEXPORT bool __stdcall CheckFace(FaceType& inFace)
{
	//Rect
	if (!CheckFaceRect(inFace.faceRect) || !CheckFaceRect(inFace.roi))
	{
		return false;
	}
	//Point
	if (!CheckBasicPos(inFace.basicPos))
	{
		return false;
	}
	//vector<rect>
	if (!CheckFaceSize(inFace.faces))
	{
		return false;
	}
	//Mat
	if (!CheckMat(inFace.mosaic) || !CheckMat(inFace.detection_frame) || !CheckMat(inFace.canny))
	{
		return false;
	}
	//int flag
	if (!CheckFaceFlag(inFace.basic_flag) || !CheckFaceFlag(inFace.detection_flag))
	{
		return false;
	}
	return true;
}

DLLEXPORT void __stdcall ClearFace(FaceType& inFace)
{
	//vector<rect>
	inFace.faces.clear();
	inFace.faces.resize(0);

	//rect
	inFace.faceRect.x = inFace.faceRect.y = inFace.faceRect.width = inFace.faceRect.height = 0;
	inFace.roi.x = inFace.roi.y = inFace.roi.width = inFace.roi.height = 0;

	//int flag
	inFace.basic_flag = inFace.detection_flag = 0;

	//mat
	inFace.mosaic.release();
	inFace.detection_frame.release();
	inFace.canny.release();

	inFace.hsv.release();
	inFace.hue.release();

	inFace.old_frame.release();

	//Point
	inFace.basicPos.x = inFace.basicPos.y = 0;
}

// プリコンパイル済みヘッダーを使用している場合、コンパイルを成功させるにはこのソース ファイルが必要です。

