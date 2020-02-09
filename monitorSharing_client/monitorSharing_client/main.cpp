#include "client_socket_class.h"

#include<opencv\cv.h>
#include<opencv\highgui.h>

using namespace cv;

void image_viewer() {
	::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);

	// ������ 10���� ���÷��� �����Ͽ� ������ϴ� ����

	String filename = "ScreenCapture.jpeg";

	Mat origin_img = imread(filename, CV_LOAD_IMAGE_COLOR);
	// �̹��� �б�
	Mat resizing_img;
	// ũ�Ⱑ ������ �̹���

	resize(origin_img, resizing_img, Size(GetSystemMetrics(SM_CXSCREEN) / 2 , GetSystemMetrics(SM_CYSCREEN) / 2 ), 0, 0, CV_INTER_LINEAR);
	imshow("viewer", resizing_img);

	waitKey(1);
	// 1ms ���� ������ ǥ��
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