#include "client_socket_class.h"

#include<opencv\cv.h>
#include<opencv\highgui.h>

using namespace cv;

void image_viewer() {
	::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);

	// 윈도우 10에서 디스플레이 관련하여 해줘야하는 설정

	String filename = "ScreenCapture.jpeg";

	Mat origin_img = imread(filename, CV_LOAD_IMAGE_COLOR);
	// 이미지 읽기
	Mat resizing_img;
	// 크기가 수정된 이미지

	resize(origin_img, resizing_img, Size(GetSystemMetrics(SM_CXSCREEN) / 2 , GetSystemMetrics(SM_CYSCREEN) / 2 ), 0, 0, CV_INTER_LINEAR);
	imshow("viewer", resizing_img);

	waitKey(1);
	// 1ms 동안 프레임 표시
}



int main() {
	client_socket_class client_socket_object;

	client_socket_object.connect_server();


	while (1) {
		client_socket_object.recvfile();
		image_viewer();
	}

	return 0;
}