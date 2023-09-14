import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YourArticlesComponent } from './your-articles.component';

describe('YourArticlesComponent', () => {
  let component: YourArticlesComponent;
  let fixture: ComponentFixture<YourArticlesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [YourArticlesComponent]
    });
    fixture = TestBed.createComponent(YourArticlesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
