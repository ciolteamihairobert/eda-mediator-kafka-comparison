import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BaseChartDirective } from 'ng2-charts';
import { SectionCardComponent } from '../../shared/section-card/section-card.component';
import { KpiCardComponent } from '../../shared/kpi-card/kpi-card.component';

type Scenario = 'baseline' | 'high-load' | 'fault';

@Component({
  selector: 'app-dashboard-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, BaseChartDirective, SectionCardComponent, KpiCardComponent],
  templateUrl: './dashboard-admin.component.html',
  styleUrl: './dashboard-admin.component.scss'
})
export class DashboardAdminComponent {
  status: 'Idle' | 'Running' | 'Stopped' | 'FaultInjected' = 'Idle';
  scenario: Scenario = 'baseline';
  selectedStack: 'dotnet' | 'go' | 'both' = 'both';

  kpis = [
    { label: 'System', value: 'Ready', hint: 'local dev' },
    { label: 'Kafka', value: 'Unknown', hint: 'hook later' },
    { label: 'Scenario', value: 'baseline', hint: 'current' },
    { label: 'Mode', value: 'Admin', hint: 'controls enabled' },
  ];

  audit: { ts: string; msg: string }[] = [
    { ts: '—', msg: 'No actions yet' }
  ];

  // mock charts (same style ca user)
  latencyCompare = {
    labels: ['P95 (ms)', 'P99 (ms)'],
    datasets: [
      { data: [42, 75], label: '.NET' },
      { data: [35, 60], label: 'Go' }
    ]
  };

  throughputCompare = {
    labels: ['Throughput (rps)'],
    datasets: [
      { data: [980], label: '.NET' },
      { data: [1120], label: 'Go' }
    ]
  };

  resourceCompare = {
    labels: ['CPU (%)', 'Memory (MB)'],
    datasets: [
      { data: [62, 420], label: '.NET' },
      { data: [48, 310], label: 'Go' }
    ]
  };

  chartOptions = {
    responsive: true,
    maintainAspectRatio: false
  };

  private log(msg: string) {
    const ts = new Date().toISOString();
    if (this.audit[0]?.msg === 'No actions yet') this.audit = [];
    this.audit = [{ ts, msg }, ...this.audit].slice(0, 12);
  }

  start() {
    this.status = 'Running';
    this.kpis = this.kpis.map(k => k.label === 'Scenario' ? ({ ...k, value: this.scenario }) : k);
    this.log(`Start benchmark (${this.scenario}, stack=${this.selectedStack})`);
  }

  stop() {
    this.status = 'Stopped';
    this.log('Stop benchmark');
  }

  reset() {
    this.status = 'Idle';
    this.log('Reset state');
  }

  injectFault(kind: 'delay' | 'restart' | 'broker') {
    this.status = 'FaultInjected';
    this.log(`Inject fault: ${kind}`);
  }

  export(kind: 'csv' | 'json') {
    this.log(`Export ${kind.toUpperCase()} (placeholder)`);
  }

  openGrafana() {
    this.log('Open Grafana (placeholder)');
  }
}
