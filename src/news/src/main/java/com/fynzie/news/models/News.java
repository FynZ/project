package com.fynzie.news.models;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

import javax.persistence.CascadeType;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToMany;
import javax.persistence.Table;

/**
* news
*/
@Entity
@Table(name = "t_news")
public class News
{
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(name = "id")
    private long id;
    
    @Column(name = "title", nullable = false)
    private String title;
    
    @Column(name= "slug", nullable = false)
    private String slug;
    
    @Column(name = "content", nullable = false)
    private String content;
    
    @Column(name = "user_id", nullable = false)
    private long userCreationId;
    
    @Column(name = "creation_date", nullable = false)
    // @Temporal(TemporalType.TIMESTAMP)
    private LocalDateTime creationDate;
    
    @Column(name = "modification_date", nullable = false)
    // @Temporal(TemporalType.TIMESTAMP)
    private LocalDateTime modificationDate;
    
    @OneToMany(
        cascade = CascadeType.ALL,
        orphanRemoval = true
    )
    @JoinColumn(name = "news_id")
    private List<Comment> comments;
    
    public News()
    {
        
    }
    
    public News(String title, String content, long userCreationId)
    {
        this.title = title;
        this.content = content;
        this.userCreationId = userCreationId;
        
        this.creationDate = LocalDateTime.now();
        this.modificationDate = LocalDateTime.now();
        this.comments = new ArrayList<Comment>();
    }
    
    public long getId()
    {
        return this.id;
    }
    
    public void setId(long id)
    {
        this.id = id;
    }
    
    public String getTitle()
    {
        return this.title;
    }
    
    public void setTitle(String title)
    {
        this.title = title;
    }
    
    public String getSlug()
    {
        return this.slug;
    }
    
    public void setSlug(String slug)
    {
        this.slug = slug;
    }
    
    public String getContent()
    {
        return this.content;
    }
    
    public void setContent(String content)
    {
        this.content = content;
    }
    
    public long getUserCreationId()
    {
        return this.userCreationId;
    }
    
    public void setUserCreationId(long userCreationId)
    {
        this.userCreationId = userCreationId;
    }
    
    public LocalDateTime getCreationDate()
    {
        return this.creationDate;
    }
    
    public void setCreationDate(LocalDateTime creationDate)
    {
        this.creationDate = creationDate;
    }
    
    public LocalDateTime getModificationDate()
    {
        return this.modificationDate;
    }
    
    public void setModificationDate(LocalDateTime modificationDate)
    {
        this.modificationDate = modificationDate;
    }
    
    public List<Comment> getComments()
    {
        return this.comments;
    }
    
    public void setComments(List<Comment> comments)
    {
        this.comments = comments;
    }
}