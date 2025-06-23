import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { tokenHttpInterceptor } from './app/services/token-http-interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(
      withInterceptors([tokenHttpInterceptor])
    ),
    ...appConfig.providers
  ]
}).catch(err => console.error(err));
