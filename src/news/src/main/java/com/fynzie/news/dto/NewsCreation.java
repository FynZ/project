package com.fynzie.news.dto;

/**
 * NewsCreation
 */
public class NewsCreation
{
    private String title;

    private String content;

    private long userCreationId;

    public NewsCreation() { }

    public NewsCreation(String title, String content, long userCreationId)
    {
        this.title = title;
        this.content = content;
        this.userCreationId = userCreationId;
    }

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

	public long getUserCreationId() {
		return this.userCreationId;
	}

	public void setUserCreationId(long userCreationId) {
		this.userCreationId = userCreationId;
	}

}