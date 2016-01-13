//
// MainPage.xaml.cpp
// Implementation of the MainPage class.
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace JsRT_UWP_Sample;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

MainPage::MainPage()
{
	InitializeComponent();
	wstring message = host.init();
	if (message != L"NoError") {
		String^ output = ref new String(message.c_str(), message.length());
		JsOutput->Text = output;
	}
}

void JsRT_UWP_Sample::MainPage::Button_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	std::wstring script(JsInput->Text->Begin());
	wstring result = host.runScript(script);
	String^ output = ref new String(result.c_str(), result.length());
	JsOutput->Text = JsOutput->Text + "\n> " + JsInput->Text + "\n" + output;
	JsOutput->UpdateLayout();
	JsOutputScroll->ScrollToVerticalOffset(DBL_MAX);
}
