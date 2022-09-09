import { Component, OnInit } from '@angular/core';

import { ToastrService } from "ngx-toastr";

@Component({
    selector: 'app-welcome-component',
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent implements OnInit {
    title = 'Zencode ng template';

    constructor(private toastrService: ToastrService) { }

    ngOnInit(): void {
        this.toastrService.info("WELCOME TO ZENCODE");
        this.toastrService.error("WELCOME TO ZENCODE");
        this.toastrService.warning("WELCOME TO ZENCODE");
        this.toastrService.success("WELCOME TO ZENCODE");
    }


}