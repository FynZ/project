package com.fynzie.news.dto;

import java.time.LocalDateTime;

/**
 * NewsSummary
 */
public class NewsSummary
{
    private String title;
    
    private String slug;
    
    private String content;
    
    private long userCreationId;
    
    private LocalDateTime creationDate;
    
    private LocalDateTime modificationDate;

    public NewsSummary() { }

	public String getTitle() {
		return this.title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public String getSlug() {
		return this.slug;
	}

	public void setSlug(String slug) {
		this.slug = slug;
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

	public LocalDateTime getCreationDate() {
		return this.creationDate;
	}

	public void setCreationDate(LocalDateTime creationDate) {
		this.creationDate = creationDate;
	}

	public LocalDateTime getModificationDate() {
		return this.modificationDate;
	}

	public void setModificationDate(LocalDateTime modificationDate) {
		this.modificationDate = modificationDate;
	}
}