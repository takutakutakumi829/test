// dllmain.cpp : DLL アプリケーションのエントリ ポイントを定義します。
#include "pch.h"
#include <stdio.h>
#include <sstream>
#include <fstream>
#include <iostream>
#include <cmath>
#include <string.h>

FaceType face;


BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    return true;
}

//文字列変換
DLLEXPORT std::wstring __stdcall ChangeToWString(std::string str)
{
	std::wstring wstr;
	auto wchar = MultiByteToWideChar(CP_ACP, 0, str.c_str(), -1, nullptr, 0);
	wstr.resize(wchar);
	auto wstring = MultiByteToWideChar(CP_ACP, 0, str.c_str(), -1, &wstr[0], wstr.size());
	return wstr;
}

DLLEXPORT std::string __stdcall ChangeToString(char* name)
{
	std::string str = { name };
	return str;
}

//stringのchar*型変換
DLLEXPORT char* ChangeToChar(std::string str_name)
{
	char* char_name = new char[str_name.size() + 1];
	strcpy_s(char_name, str_name.size() + 1, str_name.c_str());
	return char_name;
}


//拡張子の探索
DLLEXPORT char* __stdcall GetExtension(char* name)
{
	std::string path = name;
	int idx = path.find_last_of('.');
	auto str = path.substr(idx + 1, path.length() - idx);
	char* value = new char[str.size() + 1];
	strcpy_s(value, str.size() + 1, str.c_str());
	return value;
}

//拡張子外の探索
DLLEXPORT char* __stdcall GetOutExtension(char* name)
{
	std::string path = name;
	int idx = path.find_last_of('.');
	auto str = path.substr(0, idx + 1);
	char* value = new char[str.size() + 1];
	strcpy_s(value, str.size() + 1, str.c_str());
	return value;
}

//拡張子の変更
DLLEXPORT char* __stdcall ChangeExtension(char* name, char* exten)
{
	std::string outPath = GetOutExtension(name);
	std::string inPath = exten;
	std::string constValue = outPath + inPath;

	char* value = new char[constValue.size() + 1];
	strcpy_s(value, constValue.size() + 1, constValue.c_str());
	return value;
}

//拡張子の判定
DLLEXPORT bool __stdcall ExtensionCheck(char* name)
{
	std::string str = GetExtension(name);
	if (str == "png")
	{

	}
	else if (str == "bmp")
	{

	}
	else if (str == "jpg")
	{

	}
	else
	{

	}

	return false;
}

DLLEXPORT VECTOR2 __stdcall AddValue(const VECTOR2 target, const float v)
{
	VECTOR2 value;
	value.x = target.x + v;
	value.y = target.y + v;
	return value;
}

DLLEXPORT VECTOR2 __stdcall SubValue(const VECTOR2 target, const float v)
{
	VECTOR2 value;
	value.x = target.x - v;
	value.y = target.y - v;
	return value;
}

DLLEXPORT float _stdcall Magnitude(float a, float b)
{
	return sqrt(a * a + b * b);
}


DLLEXPORT cv::Point2i _stdcall Normalize(cv::Point2i a)
{
    cv::Point2i vec;
    auto z = Magnitude(a.x, a.y);
    vec.x = a.x / z;
    vec.y = a.y / z;
    return vec;
}

DLLEXPORT float _stdcall Lerp(float a, float b, float t)
{
	return a + t * (b - a);
}

DLLEXPORT VECTOR2 _stdcall GetLerpPoint(float t, VECTOR2 v1, VECTOR2 v2, VECTOR2 v3)
{
	float tp = 1 - t;
	VECTOR2 vec;
	vec.x = t * t * v3.x + 2 * t * tp * v2.x + tp * tp * v1.x;
	vec.y = t * t * v3.y + 2 * t * tp * v2.y + tp * tp * v1.y;
	return vec;
};

DLLEXPORT float __stdcall Mul(float a, float b)
{
	return a * b;
}

DLLEXPORT float __stdcall Div(float a, float b)
{
	return a / b;
}

DLLEXPORT VECTOR2 _stdcall Multiply(VECTOR2 a, float mul)
{
	VECTOR2 vec;
	vec.x = a.x * mul;
	vec.y = a.y * mul;
	return vec;
}

DLLEXPORT VECTOR2 __stdcall Divisionary(VECTOR2 a, float div)
{
	VECTOR2 vec;
	vec.x = a.x / div;
	vec.y = a.y / div;
	return vec;
}

DLLEXPORT float _stdcall GetBezier(float x, VECTOR2 pointA, VECTOR2 pointB, int n = 8)
{
	auto a = pointA;
	auto b = pointB;
	if (a.x == a.y && b.x == b.y)
	{
		return x;
	}

	float t = x;
	const float tk0 = 1 + 3 * a.x - 3 * b.x;
	const float tk1 = 3 * b.x - 6 * a.x;
	const float tk2 = 3 * a.x;

	const float epsilon = 0.0005f;
	float at = 0.0f;
	float bt = 1.0f;

	for (int i = 0; i < n; ++i)
	{
		auto ft = (tk0 * t * t * t) + (tk1 * t * t) + (tk2 * t) - x;
		if (ft <= epsilon && ft >= -epsilon)
		{
			break;
		}
		auto ftd = (3 * tk0 * t * t) + (2 * tk1 * t) + tk2;
		if (ftd == 0)
		{
			break;
		}
		t -= ft / ftd;
	}
	auto r = 1 - t;
	auto sum = t * t * t + 3 * t * t * r * b.y + 3 * t * r * r * a.y;

	return sum;
}

//画像のリサイズ
DLLEXPORT void __stdcall ImageResize(cv::Mat& image, cv::Size& oldSize)
{
	resize(image, image, cv::Size(image.cols / 10, image.rows / 10));
	resize(image, image, oldSize, 0.0, 0.0, cv::INTER_NEAREST);
}

DLLEXPORT void __stdcall FaceCheck(std::vector<cv::Rect>& obj, cv::CascadeClassifier& cas, cv::Mat& mosaic, cv::Mat& d_f, cv::Mat& f, int& detection_flag, int basic_flag, cv::Rect& rect, cv::Point& basicPos)
{
	if (detection_flag == 0)
	{
		d_f = f;
		basic_flag = 0;
		basicPos.x = 0;
		basicPos.y = 0;
	}
	else
	{
		cv::Rect roi(cv::Point(rect.x - 50, rect.y - 50), cv::Point(rect.width + 50, rect.height + 50));
		if (roi.x < 0 || roi.y < 0)
		{
			roi.x = 51;
			roi.y = 51;
			return;
		}
		d_f = f(roi);

		//検出範囲の描画
		rectangle(f, roi, cv::Scalar(200, 0, 255), 3);
		basic_flag = 1;
	}

	detection_flag = 0;

	cas.detectMultiScale(d_f, obj, 1.11, 5, 0, cv::Size(20, 20));
	if (obj.size() > 0)
	{
		for (auto o : obj)
		{

			detection_flag = 1;

			if (basic_flag == 0)
			{
				rect.x = o.x;
				rect.y = o.y;
			}
			else if (basic_flag == 1)
			{
				rect.x = (basicPos.x - 50) + o.x;
				rect.y = (basicPos.y - 50) + o.y;
			}

			rect.width = rect.x + o.width;
			rect.height = rect.y + o.height;

			basicPos.x = rect.x;
			basicPos.y = rect.y;

			//検出範囲から顔を認識してその範囲の可視化
			rectangle(f, cv::Point(rect.x, rect.y), cv::Point(rect.width, rect.height), cv::Scalar(0, 255, 0), 3);

			//認識した顔の範囲を切り取り別の画像にする
			cv::Size mosaicSize = cv::Size(rect.width - rect.x, rect.height - rect.y);
			cv::Size frameSize = cv::Size(f.cols, f.rows);

			mosaic = cv::Mat(f, cv::Rect(rect.x, rect.y, mosaicSize.width, mosaicSize.height));

			cv::Mat mat = (cv::Mat_<double>(2, 3) << 1.0, 0.0, rect.x, 0.0, 1.0, rect.y);


			//画像のリサイズ
			ImageResize(mosaic, mosaicSize);

			//アフィン変換でモザイク画を元の画像の上に描画
			warpAffine(mosaic, f, mat, f.size(), cv::INTER_LINEAR, cv::BORDER_TRANSPARENT);
		}
	}
}

DLLEXPORT void __stdcall HandDetect(cv::Mat& inPutFrame, cv::Mat& outPutFrame)
{

	cv::Mat find;
	//肌色の抽出
	cv::cvtColor(inPutFrame, face.hsv, cv::COLOR_BGR2HSV);
	cv::extractChannel(face.hsv, face.hue, 0);
	cv::inRange(face.hue, 2, 10, face.hue);

	cv::GaussianBlur(face.hue, face.hue, cv::Size(1, 1), 0);
	cv::GaussianBlur(face.hue, face.hue, cv::Size(5, 5), 0);
	cv::GaussianBlur(face.hue, face.hue, cv::Size(9, 9), 0);

	cv::copyTo(inPutFrame, outPutFrame, face.hue);

	if (face.old_frame.empty())
	{
		face.old_frame = outPutFrame;
	}

	cv::cvtColor(outPutFrame, find, cv::COLOR_BGR2GRAY);

	cv::threshold(find, find, 50, 255, cv::THRESH_BINARY);
	cv::findContours(find, face.contours, face.hierarchy, 0, 3);

	cv::drawContours(inPutFrame, face.contours, -1, cv::Scalar(0, 255, 0));

	auto fillter = [&](const std::vector<cv::Point>& vec)
	{
		return vec.size() > 100;
	};

	std::vector<std::vector<cv::Point>> vec;
	std::vector<cv::Point> mask;

	int num = 0;
	if (face.contours.size() > 0)
	{
		for (int j = 0; j < face.contours.size(); j++)
		{
			if (fillter(face.contours[j]))
			{
				vec.emplace_back();
				for (auto cont : face.contours[j])
				{
					vec[num].emplace_back(cont);
				}
				num++;
			}

		}
	}

	for (int i = 0; i < vec.size(); i++)
	{
		int npits = static_cast<int>(vec[i].size());
		cv::Point tPoint;
		for (int j = 0; j < vec[i].size(); j++)
		{
			tPoint = vec[i][j];
			if (j + 1 >= vec[i].size())
			{
				cv::line(inPutFrame, vec[i][j], vec[i][0], cv::Scalar(255, 0, 0), 2);
				break;
			}

			cv::line(inPutFrame, vec[i][j], vec[i][j + 1], cv::Scalar(255, 0, 0), 2);
			//cv::fillConvexPoly(inPutFrame, &vec[i][j], npits, cv::Scalar(255, 255, 255), 8, 0);

		}
		//cv::fillPoly(inPutFrame, vec[i], cv::Scalar(255, 255, 255));
		//cv::fillConvexPoly(inPutFrame, vec[i], cv::Scalar(255, 255, 255));
	}

	//cv::absdiff(outPutFrame, face.old_frame, inPutFrame);
	face.old_frame = outPutFrame;

}

DLLEXPORT void FocusMoment(cv::Mat& inPutFrame)
{
	cv::Mat mask1;
	cv::Mat maskFrame;
	//maskFrame = inPutFrame;
	cv::GaussianBlur(inPutFrame, maskFrame, cv::Size(15, 15), 10);
	inRange(maskFrame, cv::Scalar(0,0,50), cv::Scalar(100,120,255), mask1);

	cv::Moments mom1 = cv::moments(mask1, 1);
	cv::Point2f pt1 = cv::Point2f(mom1.m10 / mom1.m00, mom1.m01 / mom1.m00);
	cv::circle(mask1, pt1, 10, cv::Scalar(100), 3, 4);

	std::vector<std::vector<cv::Point> > h_cont;
	std::vector<cv::Vec4i> h_hie;

	cv::findContours(mask1, h_cont, h_hie, 0, 3);
	cv::drawContours(inPutFrame, h_cont, -1, cv::Scalar(0, 255, 0));

	//inPutFrame = mask1;
}

DLLEXPORT void* __stdcall CreateCascade()
{
	face.faceRect = cv::Rect(0, 0, 0, 0);
	face.detection_flag = 0;
	face.basic_flag = 0;
	face.basicPos = cv::Point(0, 0);

	return static_cast<void*>(new cv::CascadeClassifier("C:/Users/takum/OpenCV/opencv/sources/data/haarcascades/haarcascade_frontalface_alt.xml"));
}

//OpenCV使用関係
DLLEXPORT void* __stdcall GetCamera()
{
	if (!CheckFace(face))
	{
		ClearFace(face);
	}
	return static_cast<void*>(new cv::VideoCapture(0));
}

DLLEXPORT void __stdcall CloseCamera(void* camera)
{
	cv::destroyAllWindows();
	auto closeCamera = static_cast<cv::VideoCapture*>(camera);
	closeCamera->release();
}

DLLEXPORT void __stdcall CameraUpdate(void* camera, unsigned char* data, int width, int height, bool mosaic, unsigned char* imageData)
{
	auto vc = static_cast<cv::VideoCapture*>(camera);

	cv::Mat frame;
	*vc >> frame;

	cv::Mat resize_f(height, width, frame.type());
	cv::resize(frame, resize_f, resize_f.size(), cv::INTER_CUBIC);

	cv::Size oldSize;
	oldSize = cv::Size(resize_f.cols, resize_f.rows);

	face.mosaic = frame;

	//ImageResize(face.mosaic, oldSize);

	cv::imshow("window", frame);
	resize_f.release();
}

DLLEXPORT void __stdcall CameraUpdateOverRide(void* camera, int width, int height, bool mosaic, unsigned char* imageData)
{
	auto vc = static_cast<cv::VideoCapture*>(camera);
	/*cv::CascadeClassifier test;

	if (test.empty())
	{
		test.load("C:/Users/takum/OpenCV/opencv/sources/data/haarcascades/haarcascade_frontalface_alt.xml");
	}*/

	cv::Mat frame;
	cv::Mat outputFrame;
	*vc >> frame;

	cv::Mat resize_f(height, width, frame.type());
	cv::resize(frame, resize_f, resize_f.size(), cv::INTER_CUBIC);

	cv::Size oldSize;
	oldSize = cv::Size(resize_f.cols, resize_f.rows);

	face.mosaic = frame;

	/*if (!test.empty())
	{
		FaceCheck(face.faces, test, face.mosaic, face.detection_frame, frame, face.detection_flag, face.basic_flag, face.faceRect, face.basicPos);
	}*/

	FocusMoment(frame);
	HandDetect(frame, outputFrame);

	cv::imshow("window", frame);

	resize_f.release();
}


