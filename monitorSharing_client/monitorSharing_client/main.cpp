#include "client_socket_class.h"

//#include<opencv\cv.h>
//#include<opencv\highgui.h>

//using namespace cv;

/*void image_viewer() {
	::SetThreadDpiAwarenessContext(DPI_AWARENESS_CONTEXT_SYSTEM_AWARE);
	String filename = "ScreenCapture.jpeg";

	Mat origin_img = imread(filename, CV_LOAD_IMAGE_COLOR);
	Mat resizing_img;

	resize(origin_img, resizing_img, Size(GetSystemMetrics(SM_CXSCREEN) / 2, GetSystemMetrics(SM_CYSCREEN) / 2), 0, 0, CV_INTER_LINEAR);
	imshow("viewer", resizing_img);

	waitKey(10);
}*/

int main() {
	client_socket_class client_socket_object;
	while (1) {
		client_socket_object.recvfile();
		//image_viewer();
	}
	return 0;
}