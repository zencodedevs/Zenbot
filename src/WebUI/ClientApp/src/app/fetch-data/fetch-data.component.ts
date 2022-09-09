/** core imports */
import { Component } from '@angular/core';

/** external imports */
import { CityClient, CityDto } from '../shared/web-api-client';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {

  /**
   * Data  of fetch data component
   */
  public data: CityDto[];

  /**
   * Creates an instance of fetch data component.
   * @param client
   */
  constructor(private client: CityClient) {
    client.getCities().subscribe(result => {
      this.data = result;
    }, error => console.error(error));
  }
}
