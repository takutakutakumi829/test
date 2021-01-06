// pch.h: プリコンパイル済みヘッダー ファイルです。
// 次のファイルは、その後のビルドのビルド パフォーマンスを向上させるため 1 回だけコンパイルされます。
// コード補完や多くのコード参照機能などの IntelliSense パフォーマンスにも影響します。
// ただし、ここに一覧表示されているファイルは、ビルド間でいずれかが更新されると、すべてが再コンパイルされます。
// 頻繁に更新するファイルをここに追加しないでください。追加すると、パフォーマンス上の利点がなくなります。

#ifndef PCH_H
#define PCH_H

// プリコンパイルするヘッダーをここに追加します
#include "framework.h"
#include "opencv2/opencv.hpp"

#include <memory>
#include <vector>
#include <array>
#include <map>
#include <list>
#include <string>

#ifdef __cplusplus
#define DLLEXPORT extern "C" __declspec(dllexport)
#else
#define DLLEXPORT __declspec(dllexport)
#endif

typedef struct VECTOR2
{
    float x;
    float y;
};

typedef struct VECTOR3
{
    float x, y, z;

    VECTOR3(): x(0), y(0), z(0) {};
    VECTOR3(float inX, float inY, float inZ) : x(inX), y(inY), z(inZ) {};


    ~VECTOR3() {};
};


typedef struct TestStruct
{
    std::map<std::string, std::string> _mapData;
    std::string str;
    
};

typedef struct FaceType
{
    std::vector<cv::Rect> faces;
    cv::Rect faceRect = cv::Rect(0, 0, 0, 0);

    cv::Mat mosaic;
    cv::Mat detection_frame;
    cv::Mat canny;
    cv::Rect roi;
    int detection_flag = 0;

    int basic_flag = 0;
    cv::Point basicPos = cv::Point(0, 0);

};


#endif //PCH_H
