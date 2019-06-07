package com.fynzie.news.viewmodels;

/**
 * NewsViewModel
 */
public class NewsViewModel
{
    private String title;

	private String content;
	
	public NewsViewModel() { }

	public String getTitle() {
		return this.title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public String getContent() {
		return this.content;
	}

	public void setContent(String content) {
		this.content = content;
	}
}